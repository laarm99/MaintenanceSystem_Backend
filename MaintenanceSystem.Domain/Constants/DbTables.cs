namespace MaintenanceSystem.Domain.Constants
{
    /// <summary>
    /// Table names follow APP_TYPE_NAME so this app's tables can live in a shared database.
    /// </summary>
    public static class DbTables
    {
        public const string AppPrefix = "MNTSYS";

        public const string MigrationsHistory = AppPrefix + "_EF_MIGRATIONS_HISTORY";

        // IDM: identity and roles
        public const string IdmAccount = AppPrefix + "_IDM_ACCOUNT";
        public const string IdmRole = AppPrefix + "_IDM_ROLE";
        public const string IdmAccountRole = AppPrefix + "_IDM_ACCOUNT_ROLE";

        // CAT: catalogs
        public const string CatSite = AppPrefix + "_CAT_SITE";
        public const string CatPlant = AppPrefix + "_CAT_PLANT";
        public const string CatArea = AppPrefix + "_CAT_AREA";
        public const string CatEquipmentType = AppPrefix + "_CAT_EQUIPMENT_TYPE";
        public const string CatProcedure = AppPrefix + "_CAT_PROCEDURE";
        public const string CatProcedureChecklistItem = AppPrefix + "_CAT_PROCEDURE_CHECKLIST_ITEM";
        public const string CatNonWorkingDay = AppPrefix + "_CAT_NON_WORKING_DAY";

        // MNT: equipment and maintenance operation
        public const string MntEquipment = AppPrefix + "_MNT_EQUIPMENT";
        public const string MntPlan = AppPrefix + "_MNT_PLAN";
        public const string MntActivity = AppPrefix + "_MNT_ACTIVITY";
        public const string MntChecklistResult = AppPrefix + "_MNT_CHECKLIST_RESULT";
        public const string MntEvidence = AppPrefix + "_MNT_EVIDENCE";
        public const string MntReschedule = AppPrefix + "_MNT_RESCHEDULE";

        // AUD: audit
        public const string AudAuditLog = AppPrefix + "_AUD_AUDIT_LOG";
    }
}