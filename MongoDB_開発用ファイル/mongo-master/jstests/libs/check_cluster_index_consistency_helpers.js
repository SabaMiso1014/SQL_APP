/**
 * Provides a hook to check that indexes are consistent across the sharded cluster.
 *
 * The hook checks that for every collection, all the shards that own chunks for the
 * collection have the same indexes.
 */
"use strict";

load("jstests/sharding/libs/sharded_index_util.js");  // for findInconsistentIndexesAcrossShards

var ClusterIndexConsistencyChecker = (function() {
    function run(mongos, keyFile) {
        /**
         * Returns an array of config.collections docs for all collections.
         */
        function getCollDocs() {
            return mongos.getDB("config").collections.find().readConcern("local").toArray();
        }

        /**
         * Returns a function that returns an array of index docs for the namespace grouped
         * by shard.
         */
        function makeGetIndexDocsFunc(ns) {
            return () => {
                while (true) {
                    try {
                        return ShardedIndexUtil.getPerShardIndexes(mongos.getCollection(ns));
                    } catch (e) {
                        // Getting the indexes can fail with ShardNotFound if the router's
                        // ShardRegistry reloads after choosing which shards to target and a chosen
                        // shard is no longer in the cluster. This error should be transient, so it
                        // can be retried on.
                        if (e.code === ErrorCodes.ShardNotFound) {
                            print("Retrying $indexStats aggregation on ShardNotFound error: " +
                                  tojson(e));
                            continue;
                        }
                        throw e;
                    }
                }
            };
        }

        mongos.fullOptions = mongos.fullOptions || {};
        const requiresAuth = keyFile || (mongos.fullOptions.clusterAuthMode === 'x509');
        const collDocs =
            requiresAuth ? authutil.asCluster(mongos, keyFile, getCollDocs) : getCollDocs();
        for (const collDoc of collDocs) {
            const ns = collDoc._id;
            const getIndexDocsForNs = makeGetIndexDocsFunc(ns);
            print(`Checking that the indexes for ${ns} are consistent across shards...`);

            const indexDocs = requiresAuth ? authutil.asCluster(mongos, keyFile, getIndexDocsForNs)
                                           : getIndexDocsForNs();

            if (indexDocs.length == 0) {
                print(`Found no indexes for ${ns}, skipping index consistency check`);
                continue;
            }

            const inconsistentIndexes =
                ShardedIndexUtil.findInconsistentIndexesAcrossShards(indexDocs);

            for (const shard in inconsistentIndexes) {
                const shardInconsistentIndexes = inconsistentIndexes[shard];
                assert(shardInconsistentIndexes.length === 0,
                       `found inconsistent indexes for ${ns}: ${tojson(inconsistentIndexes)}`);
            }
        }
    }

    return {
        run: run,
    };
})();
