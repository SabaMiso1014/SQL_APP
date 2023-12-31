static constexpr PhaseKindTable phaseKinds = {
    /* PhaseKind::MUTATOR */ PhaseKindInfo { Phase::MUTATOR, 0, "MUTATOR" },
    /* PhaseKind::GC_BEGIN */ PhaseKindInfo { Phase::GC_BEGIN, 1, "GC_BEGIN" },
    /* PhaseKind::MARK_CCWS */ PhaseKindInfo { Phase::MARK_CCWS_1, 50, "MARK_CCWS" },
    /* PhaseKind::MARK_STACK */ PhaseKindInfo { Phase::MARK_STACK_1, 51, "MARK_STACK" },
    /* PhaseKind::MARK_RUNTIME_DATA */ PhaseKindInfo { Phase::MARK_RUNTIME_DATA_1, 52, "MARK_RUNTIME_DATA" },
    /* PhaseKind::MARK_EMBEDDING */ PhaseKindInfo { Phase::MARK_EMBEDDING_1, 53, "MARK_EMBEDDING" },
    /* PhaseKind::MARK_COMPARTMENTS */ PhaseKindInfo { Phase::MARK_COMPARTMENTS_1, 54, "MARK_COMPARTMENTS" },
    /* PhaseKind::MARK_ROOTS */ PhaseKindInfo { Phase::MARK_ROOTS_1, 48, "MARK_ROOTS" },
    /* PhaseKind::EVICT_NURSERY_FOR_MAJOR_GC */ PhaseKindInfo { Phase::EVICT_NURSERY_FOR_MAJOR_GC, 70, "EVICT_NURSERY_FOR_MAJOR_GC" },
    /* PhaseKind::WAIT_BACKGROUND_THREAD */ PhaseKindInfo { Phase::WAIT_BACKGROUND_THREAD, 2, "WAIT_BACKGROUND_THREAD" },
    /* PhaseKind::UNMARK */ PhaseKindInfo { Phase::UNMARK, 7, "UNMARK" },
    /* PhaseKind::UNMARK_WEAKMAPS */ PhaseKindInfo { Phase::UNMARK_WEAKMAPS, 76, "UNMARK_WEAKMAPS" },
    /* PhaseKind::BUFFER_GRAY_ROOTS */ PhaseKindInfo { Phase::BUFFER_GRAY_ROOTS, 49, "BUFFER_GRAY_ROOTS" },
    /* PhaseKind::MARK_DISCARD_CODE */ PhaseKindInfo { Phase::MARK_DISCARD_CODE, 3, "MARK_DISCARD_CODE" },
    /* PhaseKind::RELAZIFY_FUNCTIONS */ PhaseKindInfo { Phase::RELAZIFY_FUNCTIONS, 4, "RELAZIFY_FUNCTIONS" },
    /* PhaseKind::PURGE */ PhaseKindInfo { Phase::PURGE, 5, "PURGE" },
    /* PhaseKind::PURGE_PROP_MAP_TABLES */ PhaseKindInfo { Phase::PURGE_PROP_MAP_TABLES, 60, "PURGE_PROP_MAP_TABLES" },
    /* PhaseKind::PURGE_SOURCE_URLS */ PhaseKindInfo { Phase::PURGE_SOURCE_URLS, 73, "PURGE_SOURCE_URLS" },
    /* PhaseKind::JOIN_PARALLEL_TASKS */ PhaseKindInfo { Phase::JOIN_PARALLEL_TASKS_1, 67, "JOIN_PARALLEL_TASKS" },
    /* PhaseKind::PREPARE */ PhaseKindInfo { Phase::PREPARE, 69, "PREPARE" },
    /* PhaseKind::MARK_DELAYED */ PhaseKindInfo { Phase::MARK_DELAYED_1, 8, "MARK_DELAYED" },
    /* PhaseKind::MARK */ PhaseKindInfo { Phase::MARK, 6, "MARK" },
    /* PhaseKind::SWEEP_MARK_INCOMING_BLACK */ PhaseKindInfo { Phase::SWEEP_MARK_INCOMING_BLACK, 12, "SWEEP_MARK_INCOMING_BLACK" },
    /* PhaseKind::SWEEP_MARK_GRAY_WEAK */ PhaseKindInfo { Phase::SWEEP_MARK_GRAY_WEAK, 16, "SWEEP_MARK_GRAY_WEAK" },
    /* PhaseKind::SWEEP_MARK_WEAK */ PhaseKindInfo { Phase::SWEEP_MARK_WEAK, 13, "SWEEP_MARK_WEAK" },
    /* PhaseKind::SWEEP_MARK_INCOMING_GRAY */ PhaseKindInfo { Phase::SWEEP_MARK_INCOMING_GRAY, 14, "SWEEP_MARK_INCOMING_GRAY" },
    /* PhaseKind::SWEEP_MARK_GRAY */ PhaseKindInfo { Phase::SWEEP_MARK_GRAY, 15, "SWEEP_MARK_GRAY" },
    /* PhaseKind::SWEEP_MARK */ PhaseKindInfo { Phase::SWEEP_MARK, 10, "SWEEP_MARK" },
    /* PhaseKind::WEAK_ZONES_CALLBACK */ PhaseKindInfo { Phase::WEAK_ZONES_CALLBACK, 57, "WEAK_ZONES_CALLBACK" },
    /* PhaseKind::WEAK_COMPARTMENT_CALLBACK */ PhaseKindInfo { Phase::WEAK_COMPARTMENT_CALLBACK, 58, "WEAK_COMPARTMENT_CALLBACK" },
    /* PhaseKind::FINALIZE_START */ PhaseKindInfo { Phase::FINALIZE_START, 17, "FINALIZE_START" },
    /* PhaseKind::UPDATE_ATOMS_BITMAP */ PhaseKindInfo { Phase::UPDATE_ATOMS_BITMAP, 68, "UPDATE_ATOMS_BITMAP" },
    /* PhaseKind::SWEEP_ATOMS_TABLE */ PhaseKindInfo { Phase::SWEEP_ATOMS_TABLE, 18, "SWEEP_ATOMS_TABLE" },
    /* PhaseKind::SWEEP_DISCARD_CODE */ PhaseKindInfo { Phase::SWEEP_DISCARD_CODE, 21, "SWEEP_DISCARD_CODE" },
    /* PhaseKind::SWEEP_INNER_VIEWS */ PhaseKindInfo { Phase::SWEEP_INNER_VIEWS, 22, "SWEEP_INNER_VIEWS" },
    /* PhaseKind::SWEEP_CC_WRAPPER */ PhaseKindInfo { Phase::SWEEP_CC_WRAPPER, 23, "SWEEP_CC_WRAPPER" },
    /* PhaseKind::SWEEP_BASE_SHAPE */ PhaseKindInfo { Phase::SWEEP_BASE_SHAPE, 24, "SWEEP_BASE_SHAPE" },
    /* PhaseKind::SWEEP_INITIAL_SHAPE */ PhaseKindInfo { Phase::SWEEP_INITIAL_SHAPE, 25, "SWEEP_INITIAL_SHAPE" },
    /* PhaseKind::SWEEP_REGEXP */ PhaseKindInfo { Phase::SWEEP_REGEXP, 28, "SWEEP_REGEXP" },
    /* PhaseKind::SWEEP_COMPRESSION */ PhaseKindInfo { Phase::SWEEP_COMPRESSION, 62, "SWEEP_COMPRESSION" },
    /* PhaseKind::SWEEP_WEAKMAPS */ PhaseKindInfo { Phase::SWEEP_WEAKMAPS, 63, "SWEEP_WEAKMAPS" },
    /* PhaseKind::SWEEP_UNIQUEIDS */ PhaseKindInfo { Phase::SWEEP_UNIQUEIDS, 64, "SWEEP_UNIQUEIDS" },
    /* PhaseKind::SWEEP_FINALIZATION_REGISTRIES */ PhaseKindInfo { Phase::SWEEP_FINALIZATION_REGISTRIES, 74, "SWEEP_FINALIZATION_REGISTRIES" },
    /* PhaseKind::SWEEP_WEAKREFS */ PhaseKindInfo { Phase::SWEEP_WEAKREFS, 75, "SWEEP_WEAKREFS" },
    /* PhaseKind::SWEEP_JIT_DATA */ PhaseKindInfo { Phase::SWEEP_JIT_DATA, 65, "SWEEP_JIT_DATA" },
    /* PhaseKind::SWEEP_WEAK_CACHES */ PhaseKindInfo { Phase::SWEEP_WEAK_CACHES, 66, "SWEEP_WEAK_CACHES" },
    /* PhaseKind::SWEEP_MISC */ PhaseKindInfo { Phase::SWEEP_MISC, 29, "SWEEP_MISC" },
    /* PhaseKind::SWEEP_COMPARTMENTS */ PhaseKindInfo { Phase::SWEEP_COMPARTMENTS, 20, "SWEEP_COMPARTMENTS" },
    /* PhaseKind::SWEEP_OBJECT */ PhaseKindInfo { Phase::SWEEP_OBJECT, 33, "SWEEP_OBJECT" },
    /* PhaseKind::SWEEP_STRING */ PhaseKindInfo { Phase::SWEEP_STRING, 34, "SWEEP_STRING" },
    /* PhaseKind::SWEEP_SCRIPT */ PhaseKindInfo { Phase::SWEEP_SCRIPT, 35, "SWEEP_SCRIPT" },
    /* PhaseKind::SWEEP_SCOPE */ PhaseKindInfo { Phase::SWEEP_SCOPE, 59, "SWEEP_SCOPE" },
    /* PhaseKind::SWEEP_REGEXP_SHARED */ PhaseKindInfo { Phase::SWEEP_REGEXP_SHARED, 61, "SWEEP_REGEXP_SHARED" },
    /* PhaseKind::SWEEP_SHAPE */ PhaseKindInfo { Phase::SWEEP_SHAPE, 36, "SWEEP_SHAPE" },
    /* PhaseKind::SWEEP_PROP_MAP */ PhaseKindInfo { Phase::SWEEP_PROP_MAP, 77, "SWEEP_PROP_MAP" },
    /* PhaseKind::FINALIZE_END */ PhaseKindInfo { Phase::FINALIZE_END, 38, "FINALIZE_END" },
    /* PhaseKind::DESTROY */ PhaseKindInfo { Phase::DESTROY, 39, "DESTROY" },
    /* PhaseKind::SWEEP */ PhaseKindInfo { Phase::SWEEP, 9, "SWEEP" },
    /* PhaseKind::COMPACT_MOVE */ PhaseKindInfo { Phase::COMPACT_MOVE, 41, "COMPACT_MOVE" },
    /* PhaseKind::COMPACT_UPDATE_CELLS */ PhaseKindInfo { Phase::COMPACT_UPDATE_CELLS, 43, "COMPACT_UPDATE_CELLS" },
    /* PhaseKind::COMPACT_UPDATE */ PhaseKindInfo { Phase::COMPACT_UPDATE, 42, "COMPACT_UPDATE" },
    /* PhaseKind::COMPACT */ PhaseKindInfo { Phase::COMPACT, 40, "COMPACT" },
    /* PhaseKind::DECOMMIT */ PhaseKindInfo { Phase::DECOMMIT, 72, "DECOMMIT" },
    /* PhaseKind::GC_END */ PhaseKindInfo { Phase::GC_END, 44, "GC_END" },
    /* PhaseKind::MINOR_GC */ PhaseKindInfo { Phase::MINOR_GC, 45, "MINOR_GC" },
    /* PhaseKind::EVICT_NURSERY */ PhaseKindInfo { Phase::EVICT_NURSERY, 46, "EVICT_NURSERY" },
    /* PhaseKind::TRACE_HEAP */ PhaseKindInfo { Phase::TRACE_HEAP, 47, "TRACE_HEAP" },
    /* PhaseKind::UNMARK_GRAY */ PhaseKindInfo { Phase::UNMARK_GRAY, 56, "UNMARK_GRAY" },
    /* PhaseKind::BARRIER */ PhaseKindInfo { Phase::BARRIER, 55, "BARRIER" },
};

static constexpr PhaseTable phases = {
    /* Phase::MUTATOR */ PhaseInfo { Phase::NONE, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::MUTATOR, 0, "Mutator Running", "mutator" },
    /* Phase::GC_BEGIN */ PhaseInfo { Phase::NONE, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::GC_BEGIN, 0, "Begin Callback", "gc_begin" },
    /* Phase::EVICT_NURSERY_FOR_MAJOR_GC */ PhaseInfo { Phase::NONE, Phase::MARK_ROOTS_1, Phase::NONE, Phase::NONE, PhaseKind::EVICT_NURSERY_FOR_MAJOR_GC, 0, "Evict Nursery For Major GC", "evict_nursery_for_major_gc" },
    /* Phase::MARK_ROOTS_1 */ PhaseInfo { Phase::EVICT_NURSERY_FOR_MAJOR_GC, Phase::MARK_CCWS_1, Phase::NONE, Phase::MARK_ROOTS_2, PhaseKind::MARK_ROOTS, 1, "Mark Roots", "evict_nursery_for_major_gc.mark_roots" },
    /* Phase::MARK_CCWS_1 */ PhaseInfo { Phase::MARK_ROOTS_1, Phase::NONE, Phase::MARK_STACK_1, Phase::MARK_CCWS_2, PhaseKind::MARK_CCWS, 2, "Mark Cross Compartment Wrappers", "evict_nursery_for_major_gc.mark_roots.mark_ccws" },
    /* Phase::MARK_STACK_1 */ PhaseInfo { Phase::MARK_ROOTS_1, Phase::NONE, Phase::MARK_RUNTIME_DATA_1, Phase::MARK_STACK_2, PhaseKind::MARK_STACK, 2, "Mark C and JS stacks", "evict_nursery_for_major_gc.mark_roots.mark_stack" },
    /* Phase::MARK_RUNTIME_DATA_1 */ PhaseInfo { Phase::MARK_ROOTS_1, Phase::NONE, Phase::MARK_EMBEDDING_1, Phase::MARK_RUNTIME_DATA_2, PhaseKind::MARK_RUNTIME_DATA, 2, "Mark Runtime-wide Data", "evict_nursery_for_major_gc.mark_roots.mark_runtime_data" },
    /* Phase::MARK_EMBEDDING_1 */ PhaseInfo { Phase::MARK_ROOTS_1, Phase::NONE, Phase::MARK_COMPARTMENTS_1, Phase::MARK_EMBEDDING_2, PhaseKind::MARK_EMBEDDING, 2, "Mark Embedding", "evict_nursery_for_major_gc.mark_roots.mark_embedding" },
    /* Phase::MARK_COMPARTMENTS_1 */ PhaseInfo { Phase::MARK_ROOTS_1, Phase::NONE, Phase::NONE, Phase::MARK_COMPARTMENTS_2, PhaseKind::MARK_COMPARTMENTS, 2, "Mark Compartments", "evict_nursery_for_major_gc.mark_roots.mark_compartments" },
    /* Phase::WAIT_BACKGROUND_THREAD */ PhaseInfo { Phase::NONE, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::WAIT_BACKGROUND_THREAD, 0, "Wait Background Thread", "wait_background_thread" },
    /* Phase::PREPARE */ PhaseInfo { Phase::NONE, Phase::UNMARK, Phase::NONE, Phase::NONE, PhaseKind::PREPARE, 0, "Prepare For Collection", "prepare" },
    /* Phase::UNMARK */ PhaseInfo { Phase::PREPARE, Phase::NONE, Phase::UNMARK_WEAKMAPS, Phase::NONE, PhaseKind::UNMARK, 1, "Unmark", "prepare.unmark" },
    /* Phase::UNMARK_WEAKMAPS */ PhaseInfo { Phase::PREPARE, Phase::NONE, Phase::BUFFER_GRAY_ROOTS, Phase::NONE, PhaseKind::UNMARK_WEAKMAPS, 1, "Unmark WeakMaps", "prepare.unmark_weakmaps" },
    /* Phase::BUFFER_GRAY_ROOTS */ PhaseInfo { Phase::PREPARE, Phase::NONE, Phase::MARK_DISCARD_CODE, Phase::NONE, PhaseKind::BUFFER_GRAY_ROOTS, 1, "Buffer Gray Roots", "prepare.buffer_gray_roots" },
    /* Phase::MARK_DISCARD_CODE */ PhaseInfo { Phase::PREPARE, Phase::NONE, Phase::RELAZIFY_FUNCTIONS, Phase::NONE, PhaseKind::MARK_DISCARD_CODE, 1, "Mark Discard Code", "prepare.mark_discard_code" },
    /* Phase::RELAZIFY_FUNCTIONS */ PhaseInfo { Phase::PREPARE, Phase::NONE, Phase::PURGE, Phase::NONE, PhaseKind::RELAZIFY_FUNCTIONS, 1, "Relazify Functions", "prepare.relazify_functions" },
    /* Phase::PURGE */ PhaseInfo { Phase::PREPARE, Phase::NONE, Phase::PURGE_PROP_MAP_TABLES, Phase::NONE, PhaseKind::PURGE, 1, "Purge", "prepare.purge" },
    /* Phase::PURGE_PROP_MAP_TABLES */ PhaseInfo { Phase::PREPARE, Phase::NONE, Phase::PURGE_SOURCE_URLS, Phase::NONE, PhaseKind::PURGE_PROP_MAP_TABLES, 1, "Purge PropMapTables", "prepare.purge_prop_map_tables" },
    /* Phase::PURGE_SOURCE_URLS */ PhaseInfo { Phase::PREPARE, Phase::NONE, Phase::JOIN_PARALLEL_TASKS_1, Phase::NONE, PhaseKind::PURGE_SOURCE_URLS, 1, "Purge Source URLs", "prepare.purge_source_urls" },
    /* Phase::JOIN_PARALLEL_TASKS_1 */ PhaseInfo { Phase::PREPARE, Phase::NONE, Phase::NONE, Phase::JOIN_PARALLEL_TASKS_2, PhaseKind::JOIN_PARALLEL_TASKS, 1, "Join Parallel Tasks", "prepare.join_parallel_tasks" },
    /* Phase::MARK */ PhaseInfo { Phase::NONE, Phase::MARK_ROOTS_2, Phase::NONE, Phase::NONE, PhaseKind::MARK, 0, "Mark", "mark" },
    /* Phase::MARK_ROOTS_2 */ PhaseInfo { Phase::MARK, Phase::MARK_CCWS_2, Phase::MARK_DELAYED_1, Phase::MARK_ROOTS_3, PhaseKind::MARK_ROOTS, 1, "Mark Roots", "mark.mark_roots" },
    /* Phase::MARK_CCWS_2 */ PhaseInfo { Phase::MARK_ROOTS_2, Phase::NONE, Phase::MARK_STACK_2, Phase::MARK_CCWS_3, PhaseKind::MARK_CCWS, 2, "Mark Cross Compartment Wrappers", "mark.mark_roots.mark_ccws" },
    /* Phase::MARK_STACK_2 */ PhaseInfo { Phase::MARK_ROOTS_2, Phase::NONE, Phase::MARK_RUNTIME_DATA_2, Phase::MARK_STACK_3, PhaseKind::MARK_STACK, 2, "Mark C and JS stacks", "mark.mark_roots.mark_stack" },
    /* Phase::MARK_RUNTIME_DATA_2 */ PhaseInfo { Phase::MARK_ROOTS_2, Phase::NONE, Phase::MARK_EMBEDDING_2, Phase::MARK_RUNTIME_DATA_3, PhaseKind::MARK_RUNTIME_DATA, 2, "Mark Runtime-wide Data", "mark.mark_roots.mark_runtime_data" },
    /* Phase::MARK_EMBEDDING_2 */ PhaseInfo { Phase::MARK_ROOTS_2, Phase::NONE, Phase::MARK_COMPARTMENTS_2, Phase::MARK_EMBEDDING_3, PhaseKind::MARK_EMBEDDING, 2, "Mark Embedding", "mark.mark_roots.mark_embedding" },
    /* Phase::MARK_COMPARTMENTS_2 */ PhaseInfo { Phase::MARK_ROOTS_2, Phase::NONE, Phase::NONE, Phase::MARK_COMPARTMENTS_3, PhaseKind::MARK_COMPARTMENTS, 2, "Mark Compartments", "mark.mark_roots.mark_compartments" },
    /* Phase::MARK_DELAYED_1 */ PhaseInfo { Phase::MARK, Phase::NONE, Phase::NONE, Phase::MARK_DELAYED_2, PhaseKind::MARK_DELAYED, 1, "Mark Delayed", "mark.mark_delayed" },
    /* Phase::SWEEP */ PhaseInfo { Phase::NONE, Phase::SWEEP_MARK, Phase::NONE, Phase::NONE, PhaseKind::SWEEP, 0, "Sweep", "sweep" },
    /* Phase::SWEEP_MARK */ PhaseInfo { Phase::SWEEP, Phase::MARK_DELAYED_2, Phase::FINALIZE_START, Phase::NONE, PhaseKind::SWEEP_MARK, 1, "Mark During Sweeping", "sweep.sweep_mark" },
    /* Phase::MARK_DELAYED_2 */ PhaseInfo { Phase::SWEEP_MARK, Phase::NONE, Phase::SWEEP_MARK_INCOMING_BLACK, Phase::MARK_DELAYED_3, PhaseKind::MARK_DELAYED, 2, "Mark Delayed", "sweep.sweep_mark.mark_delayed" },
    /* Phase::SWEEP_MARK_INCOMING_BLACK */ PhaseInfo { Phase::SWEEP_MARK, Phase::NONE, Phase::SWEEP_MARK_WEAK, Phase::NONE, PhaseKind::SWEEP_MARK_INCOMING_BLACK, 2, "Mark Incoming Black Pointers", "sweep.sweep_mark.sweep_mark_incoming_black" },
    /* Phase::SWEEP_MARK_WEAK */ PhaseInfo { Phase::SWEEP_MARK, Phase::MARK_DELAYED_3, Phase::SWEEP_MARK_INCOMING_GRAY, Phase::NONE, PhaseKind::SWEEP_MARK_WEAK, 2, "Mark Weak", "sweep.sweep_mark.sweep_mark_weak" },
    /* Phase::MARK_DELAYED_3 */ PhaseInfo { Phase::SWEEP_MARK_WEAK, Phase::NONE, Phase::SWEEP_MARK_GRAY_WEAK, Phase::NONE, PhaseKind::MARK_DELAYED, 3, "Mark Delayed", "sweep.sweep_mark.sweep_mark_weak.mark_delayed" },
    /* Phase::SWEEP_MARK_GRAY_WEAK */ PhaseInfo { Phase::SWEEP_MARK_WEAK, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::SWEEP_MARK_GRAY_WEAK, 3, "Mark Gray and Weak", "sweep.sweep_mark.sweep_mark_weak.sweep_mark_gray_weak" },
    /* Phase::SWEEP_MARK_INCOMING_GRAY */ PhaseInfo { Phase::SWEEP_MARK, Phase::NONE, Phase::SWEEP_MARK_GRAY, Phase::NONE, PhaseKind::SWEEP_MARK_INCOMING_GRAY, 2, "Mark Incoming Gray Pointers", "sweep.sweep_mark.sweep_mark_incoming_gray" },
    /* Phase::SWEEP_MARK_GRAY */ PhaseInfo { Phase::SWEEP_MARK, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::SWEEP_MARK_GRAY, 2, "Mark Gray", "sweep.sweep_mark.sweep_mark_gray" },
    /* Phase::FINALIZE_START */ PhaseInfo { Phase::SWEEP, Phase::WEAK_ZONES_CALLBACK, Phase::UPDATE_ATOMS_BITMAP, Phase::NONE, PhaseKind::FINALIZE_START, 1, "Finalize Start Callbacks", "sweep.finalize_start" },
    /* Phase::WEAK_ZONES_CALLBACK */ PhaseInfo { Phase::FINALIZE_START, Phase::NONE, Phase::WEAK_COMPARTMENT_CALLBACK, Phase::NONE, PhaseKind::WEAK_ZONES_CALLBACK, 2, "Per-Slice Weak Callback", "sweep.finalize_start.weak_zones_callback" },
    /* Phase::WEAK_COMPARTMENT_CALLBACK */ PhaseInfo { Phase::FINALIZE_START, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::WEAK_COMPARTMENT_CALLBACK, 2, "Per-Compartment Weak Callback", "sweep.finalize_start.weak_compartment_callback" },
    /* Phase::UPDATE_ATOMS_BITMAP */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::SWEEP_ATOMS_TABLE, Phase::NONE, PhaseKind::UPDATE_ATOMS_BITMAP, 1, "Sweep Atoms Bitmap", "sweep.update_atoms_bitmap" },
    /* Phase::SWEEP_ATOMS_TABLE */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::SWEEP_COMPARTMENTS, Phase::NONE, PhaseKind::SWEEP_ATOMS_TABLE, 1, "Sweep Atoms Table", "sweep.sweep_atoms_table" },
    /* Phase::SWEEP_COMPARTMENTS */ PhaseInfo { Phase::SWEEP, Phase::SWEEP_DISCARD_CODE, Phase::SWEEP_OBJECT, Phase::NONE, PhaseKind::SWEEP_COMPARTMENTS, 1, "Sweep Compartments", "sweep.sweep_compartments" },
    /* Phase::SWEEP_DISCARD_CODE */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_INNER_VIEWS, Phase::NONE, PhaseKind::SWEEP_DISCARD_CODE, 2, "Sweep Discard Code", "sweep.sweep_compartments.sweep_discard_code" },
    /* Phase::SWEEP_INNER_VIEWS */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_CC_WRAPPER, Phase::NONE, PhaseKind::SWEEP_INNER_VIEWS, 2, "Sweep Inner Views", "sweep.sweep_compartments.sweep_inner_views" },
    /* Phase::SWEEP_CC_WRAPPER */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_BASE_SHAPE, Phase::NONE, PhaseKind::SWEEP_CC_WRAPPER, 2, "Sweep Cross Compartment Wrappers", "sweep.sweep_compartments.sweep_cc_wrapper" },
    /* Phase::SWEEP_BASE_SHAPE */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_INITIAL_SHAPE, Phase::NONE, PhaseKind::SWEEP_BASE_SHAPE, 2, "Sweep Base Shapes", "sweep.sweep_compartments.sweep_base_shape" },
    /* Phase::SWEEP_INITIAL_SHAPE */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_REGEXP, Phase::NONE, PhaseKind::SWEEP_INITIAL_SHAPE, 2, "Sweep Initial Shapes", "sweep.sweep_compartments.sweep_initial_shape" },
    /* Phase::SWEEP_REGEXP */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_COMPRESSION, Phase::NONE, PhaseKind::SWEEP_REGEXP, 2, "Sweep Regexps", "sweep.sweep_compartments.sweep_regexp" },
    /* Phase::SWEEP_COMPRESSION */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_WEAKMAPS, Phase::NONE, PhaseKind::SWEEP_COMPRESSION, 2, "Sweep Compression Tasks", "sweep.sweep_compartments.sweep_compression" },
    /* Phase::SWEEP_WEAKMAPS */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_UNIQUEIDS, Phase::NONE, PhaseKind::SWEEP_WEAKMAPS, 2, "Sweep WeakMaps", "sweep.sweep_compartments.sweep_weakmaps" },
    /* Phase::SWEEP_UNIQUEIDS */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_FINALIZATION_REGISTRIES, Phase::NONE, PhaseKind::SWEEP_UNIQUEIDS, 2, "Sweep Unique IDs", "sweep.sweep_compartments.sweep_uniqueids" },
    /* Phase::SWEEP_FINALIZATION_REGISTRIES */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_WEAKREFS, Phase::NONE, PhaseKind::SWEEP_FINALIZATION_REGISTRIES, 2, "Sweep FinalizationRegistries", "sweep.sweep_compartments.sweep_finalization_registries" },
    /* Phase::SWEEP_WEAKREFS */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_JIT_DATA, Phase::NONE, PhaseKind::SWEEP_WEAKREFS, 2, "Sweep WeakRefs", "sweep.sweep_compartments.sweep_weakrefs" },
    /* Phase::SWEEP_JIT_DATA */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_WEAK_CACHES, Phase::NONE, PhaseKind::SWEEP_JIT_DATA, 2, "Sweep JIT Data", "sweep.sweep_compartments.sweep_jit_data" },
    /* Phase::SWEEP_WEAK_CACHES */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::SWEEP_MISC, Phase::NONE, PhaseKind::SWEEP_WEAK_CACHES, 2, "Sweep Weak Caches", "sweep.sweep_compartments.sweep_weak_caches" },
    /* Phase::SWEEP_MISC */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::JOIN_PARALLEL_TASKS_2, Phase::NONE, PhaseKind::SWEEP_MISC, 2, "Sweep Miscellaneous", "sweep.sweep_compartments.sweep_misc" },
    /* Phase::JOIN_PARALLEL_TASKS_2 */ PhaseInfo { Phase::SWEEP_COMPARTMENTS, Phase::NONE, Phase::NONE, Phase::JOIN_PARALLEL_TASKS_3, PhaseKind::JOIN_PARALLEL_TASKS, 2, "Join Parallel Tasks", "sweep.sweep_compartments.join_parallel_tasks" },
    /* Phase::SWEEP_OBJECT */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::SWEEP_STRING, Phase::NONE, PhaseKind::SWEEP_OBJECT, 1, "Sweep Object", "sweep.sweep_object" },
    /* Phase::SWEEP_STRING */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::SWEEP_SCRIPT, Phase::NONE, PhaseKind::SWEEP_STRING, 1, "Sweep String", "sweep.sweep_string" },
    /* Phase::SWEEP_SCRIPT */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::SWEEP_SCOPE, Phase::NONE, PhaseKind::SWEEP_SCRIPT, 1, "Sweep Script", "sweep.sweep_script" },
    /* Phase::SWEEP_SCOPE */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::SWEEP_REGEXP_SHARED, Phase::NONE, PhaseKind::SWEEP_SCOPE, 1, "Sweep Scope", "sweep.sweep_scope" },
    /* Phase::SWEEP_REGEXP_SHARED */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::SWEEP_SHAPE, Phase::NONE, PhaseKind::SWEEP_REGEXP_SHARED, 1, "Sweep RegExpShared", "sweep.sweep_regexp_shared" },
    /* Phase::SWEEP_SHAPE */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::SWEEP_PROP_MAP, Phase::NONE, PhaseKind::SWEEP_SHAPE, 1, "Sweep Shape", "sweep.sweep_shape" },
    /* Phase::SWEEP_PROP_MAP */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::FINALIZE_END, Phase::NONE, PhaseKind::SWEEP_PROP_MAP, 1, "Sweep PropMap", "sweep.sweep_prop_map" },
    /* Phase::FINALIZE_END */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::DESTROY, Phase::NONE, PhaseKind::FINALIZE_END, 1, "Finalize End Callback", "sweep.finalize_end" },
    /* Phase::DESTROY */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::JOIN_PARALLEL_TASKS_3, Phase::NONE, PhaseKind::DESTROY, 1, "Deallocate", "sweep.destroy" },
    /* Phase::JOIN_PARALLEL_TASKS_3 */ PhaseInfo { Phase::SWEEP, Phase::NONE, Phase::NONE, Phase::JOIN_PARALLEL_TASKS_4, PhaseKind::JOIN_PARALLEL_TASKS, 1, "Join Parallel Tasks", "sweep.join_parallel_tasks" },
    /* Phase::COMPACT */ PhaseInfo { Phase::NONE, Phase::COMPACT_MOVE, Phase::NONE, Phase::NONE, PhaseKind::COMPACT, 0, "Compact", "compact" },
    /* Phase::COMPACT_MOVE */ PhaseInfo { Phase::COMPACT, Phase::NONE, Phase::COMPACT_UPDATE, Phase::NONE, PhaseKind::COMPACT_MOVE, 1, "Compact Move", "compact.compact_move" },
    /* Phase::COMPACT_UPDATE */ PhaseInfo { Phase::COMPACT, Phase::MARK_ROOTS_3, Phase::NONE, Phase::NONE, PhaseKind::COMPACT_UPDATE, 1, "Compact Update", "compact.compact_update" },
    /* Phase::MARK_ROOTS_3 */ PhaseInfo { Phase::COMPACT_UPDATE, Phase::MARK_CCWS_3, Phase::COMPACT_UPDATE_CELLS, Phase::MARK_ROOTS_4, PhaseKind::MARK_ROOTS, 2, "Mark Roots", "compact.compact_update.mark_roots" },
    /* Phase::MARK_CCWS_3 */ PhaseInfo { Phase::MARK_ROOTS_3, Phase::NONE, Phase::MARK_STACK_3, Phase::MARK_CCWS_4, PhaseKind::MARK_CCWS, 3, "Mark Cross Compartment Wrappers", "compact.compact_update.mark_roots.mark_ccws" },
    /* Phase::MARK_STACK_3 */ PhaseInfo { Phase::MARK_ROOTS_3, Phase::NONE, Phase::MARK_RUNTIME_DATA_3, Phase::MARK_STACK_4, PhaseKind::MARK_STACK, 3, "Mark C and JS stacks", "compact.compact_update.mark_roots.mark_stack" },
    /* Phase::MARK_RUNTIME_DATA_3 */ PhaseInfo { Phase::MARK_ROOTS_3, Phase::NONE, Phase::MARK_EMBEDDING_3, Phase::MARK_RUNTIME_DATA_4, PhaseKind::MARK_RUNTIME_DATA, 3, "Mark Runtime-wide Data", "compact.compact_update.mark_roots.mark_runtime_data" },
    /* Phase::MARK_EMBEDDING_3 */ PhaseInfo { Phase::MARK_ROOTS_3, Phase::NONE, Phase::MARK_COMPARTMENTS_3, Phase::MARK_EMBEDDING_4, PhaseKind::MARK_EMBEDDING, 3, "Mark Embedding", "compact.compact_update.mark_roots.mark_embedding" },
    /* Phase::MARK_COMPARTMENTS_3 */ PhaseInfo { Phase::MARK_ROOTS_3, Phase::NONE, Phase::NONE, Phase::MARK_COMPARTMENTS_4, PhaseKind::MARK_COMPARTMENTS, 3, "Mark Compartments", "compact.compact_update.mark_roots.mark_compartments" },
    /* Phase::COMPACT_UPDATE_CELLS */ PhaseInfo { Phase::COMPACT_UPDATE, Phase::NONE, Phase::JOIN_PARALLEL_TASKS_4, Phase::NONE, PhaseKind::COMPACT_UPDATE_CELLS, 2, "Compact Update Cells", "compact.compact_update.compact_update_cells" },
    /* Phase::JOIN_PARALLEL_TASKS_4 */ PhaseInfo { Phase::COMPACT_UPDATE, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::JOIN_PARALLEL_TASKS, 2, "Join Parallel Tasks", "compact.compact_update.join_parallel_tasks" },
    /* Phase::DECOMMIT */ PhaseInfo { Phase::NONE, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::DECOMMIT, 0, "Decommit", "decommit" },
    /* Phase::GC_END */ PhaseInfo { Phase::NONE, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::GC_END, 0, "End Callback", "gc_end" },
    /* Phase::MINOR_GC */ PhaseInfo { Phase::NONE, Phase::MARK_ROOTS_4, Phase::NONE, Phase::NONE, PhaseKind::MINOR_GC, 0, "All Minor GCs", "minor_gc" },
    /* Phase::MARK_ROOTS_4 */ PhaseInfo { Phase::MINOR_GC, Phase::MARK_CCWS_4, Phase::NONE, Phase::MARK_ROOTS_5, PhaseKind::MARK_ROOTS, 1, "Mark Roots", "minor_gc.mark_roots" },
    /* Phase::MARK_CCWS_4 */ PhaseInfo { Phase::MARK_ROOTS_4, Phase::NONE, Phase::MARK_STACK_4, Phase::MARK_CCWS_5, PhaseKind::MARK_CCWS, 2, "Mark Cross Compartment Wrappers", "minor_gc.mark_roots.mark_ccws" },
    /* Phase::MARK_STACK_4 */ PhaseInfo { Phase::MARK_ROOTS_4, Phase::NONE, Phase::MARK_RUNTIME_DATA_4, Phase::MARK_STACK_5, PhaseKind::MARK_STACK, 2, "Mark C and JS stacks", "minor_gc.mark_roots.mark_stack" },
    /* Phase::MARK_RUNTIME_DATA_4 */ PhaseInfo { Phase::MARK_ROOTS_4, Phase::NONE, Phase::MARK_EMBEDDING_4, Phase::MARK_RUNTIME_DATA_5, PhaseKind::MARK_RUNTIME_DATA, 2, "Mark Runtime-wide Data", "minor_gc.mark_roots.mark_runtime_data" },
    /* Phase::MARK_EMBEDDING_4 */ PhaseInfo { Phase::MARK_ROOTS_4, Phase::NONE, Phase::MARK_COMPARTMENTS_4, Phase::MARK_EMBEDDING_5, PhaseKind::MARK_EMBEDDING, 2, "Mark Embedding", "minor_gc.mark_roots.mark_embedding" },
    /* Phase::MARK_COMPARTMENTS_4 */ PhaseInfo { Phase::MARK_ROOTS_4, Phase::NONE, Phase::NONE, Phase::MARK_COMPARTMENTS_5, PhaseKind::MARK_COMPARTMENTS, 2, "Mark Compartments", "minor_gc.mark_roots.mark_compartments" },
    /* Phase::EVICT_NURSERY */ PhaseInfo { Phase::NONE, Phase::MARK_ROOTS_5, Phase::NONE, Phase::NONE, PhaseKind::EVICT_NURSERY, 0, "Minor GCs to Evict Nursery", "evict_nursery" },
    /* Phase::MARK_ROOTS_5 */ PhaseInfo { Phase::EVICT_NURSERY, Phase::MARK_CCWS_5, Phase::NONE, Phase::MARK_ROOTS_6, PhaseKind::MARK_ROOTS, 1, "Mark Roots", "evict_nursery.mark_roots" },
    /* Phase::MARK_CCWS_5 */ PhaseInfo { Phase::MARK_ROOTS_5, Phase::NONE, Phase::MARK_STACK_5, Phase::MARK_CCWS_6, PhaseKind::MARK_CCWS, 2, "Mark Cross Compartment Wrappers", "evict_nursery.mark_roots.mark_ccws" },
    /* Phase::MARK_STACK_5 */ PhaseInfo { Phase::MARK_ROOTS_5, Phase::NONE, Phase::MARK_RUNTIME_DATA_5, Phase::MARK_STACK_6, PhaseKind::MARK_STACK, 2, "Mark C and JS stacks", "evict_nursery.mark_roots.mark_stack" },
    /* Phase::MARK_RUNTIME_DATA_5 */ PhaseInfo { Phase::MARK_ROOTS_5, Phase::NONE, Phase::MARK_EMBEDDING_5, Phase::MARK_RUNTIME_DATA_6, PhaseKind::MARK_RUNTIME_DATA, 2, "Mark Runtime-wide Data", "evict_nursery.mark_roots.mark_runtime_data" },
    /* Phase::MARK_EMBEDDING_5 */ PhaseInfo { Phase::MARK_ROOTS_5, Phase::NONE, Phase::MARK_COMPARTMENTS_5, Phase::MARK_EMBEDDING_6, PhaseKind::MARK_EMBEDDING, 2, "Mark Embedding", "evict_nursery.mark_roots.mark_embedding" },
    /* Phase::MARK_COMPARTMENTS_5 */ PhaseInfo { Phase::MARK_ROOTS_5, Phase::NONE, Phase::NONE, Phase::MARK_COMPARTMENTS_6, PhaseKind::MARK_COMPARTMENTS, 2, "Mark Compartments", "evict_nursery.mark_roots.mark_compartments" },
    /* Phase::TRACE_HEAP */ PhaseInfo { Phase::NONE, Phase::MARK_ROOTS_6, Phase::NONE, Phase::NONE, PhaseKind::TRACE_HEAP, 0, "Trace Heap", "trace_heap" },
    /* Phase::MARK_ROOTS_6 */ PhaseInfo { Phase::TRACE_HEAP, Phase::MARK_CCWS_6, Phase::NONE, Phase::NONE, PhaseKind::MARK_ROOTS, 1, "Mark Roots", "trace_heap.mark_roots" },
    /* Phase::MARK_CCWS_6 */ PhaseInfo { Phase::MARK_ROOTS_6, Phase::NONE, Phase::MARK_STACK_6, Phase::NONE, PhaseKind::MARK_CCWS, 2, "Mark Cross Compartment Wrappers", "trace_heap.mark_roots.mark_ccws" },
    /* Phase::MARK_STACK_6 */ PhaseInfo { Phase::MARK_ROOTS_6, Phase::NONE, Phase::MARK_RUNTIME_DATA_6, Phase::NONE, PhaseKind::MARK_STACK, 2, "Mark C and JS stacks", "trace_heap.mark_roots.mark_stack" },
    /* Phase::MARK_RUNTIME_DATA_6 */ PhaseInfo { Phase::MARK_ROOTS_6, Phase::NONE, Phase::MARK_EMBEDDING_6, Phase::NONE, PhaseKind::MARK_RUNTIME_DATA, 2, "Mark Runtime-wide Data", "trace_heap.mark_roots.mark_runtime_data" },
    /* Phase::MARK_EMBEDDING_6 */ PhaseInfo { Phase::MARK_ROOTS_6, Phase::NONE, Phase::MARK_COMPARTMENTS_6, Phase::NONE, PhaseKind::MARK_EMBEDDING, 2, "Mark Embedding", "trace_heap.mark_roots.mark_embedding" },
    /* Phase::MARK_COMPARTMENTS_6 */ PhaseInfo { Phase::MARK_ROOTS_6, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::MARK_COMPARTMENTS, 2, "Mark Compartments", "trace_heap.mark_roots.mark_compartments" },
    /* Phase::BARRIER */ PhaseInfo { Phase::NONE, Phase::UNMARK_GRAY, Phase::NONE, Phase::NONE, PhaseKind::BARRIER, 0, "Barriers", "barrier" },
    /* Phase::UNMARK_GRAY */ PhaseInfo { Phase::BARRIER, Phase::NONE, Phase::NONE, Phase::NONE, PhaseKind::UNMARK_GRAY, 1, "Unmark gray", "barrier.unmark_gray" },
};
// The next available phase kind number is: 78
