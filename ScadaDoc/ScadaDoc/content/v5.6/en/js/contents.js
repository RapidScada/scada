// Add table of contents in English here
function addContents(context) {
    addArticle(context, "../../../", "Home");
    addArticle(context, "software-overview/", "Software Overview");
    addArticle(context, "software-overview/software-purpose.html", "Software Purpose and Characteristics", 1);
    addArticle(context, "software-overview/software-architecture.html", "Software Architecture", 1);
    addArticle(context, "software-overview/applications/", "Description of Applications", 1);
    addArticle(context, "software-overview/applications/server-application.html", "Server Application", 2);
    addArticle(context, "software-overview/applications/communicator-application.html", "Communicator Application", 2);
    addArticle(context, "software-overview/applications/webstation-application.html", "Webstation Application", 2);
    addArticle(context, "software-overview/applications/administrator-application.html", "Administrator Application", 2);
    addArticle(context, "software-overview/applications/table-editor-application.html", "Table Editor Application", 2);
    addArticle(context, "software-overview/applications/scheme-editor-application.html", "Scheme Editor Application", 2);

    addArticle(context, "installation-and-run/", "Installation and Run");
    addArticle(context, "installation-and-run/system-requirements.html", "System Requirements", 1);
    addArticle(context, "installation-and-run/software-installation.html", "Software Installation", 1);
    addArticle(context, "installation-and-run/manual-installation.html", "Manual Installation", 1);
    addArticle(context, "installation-and-run/module-installation.html", "Installation of Additional Modules", 1);
    addArticle(context, "installation-and-run/run-applications.html", "Run Applications", 1);
    addArticle(context, "installation-and-run/migrate-configuration.html", "Migrate Configuration to New Server", 1);
    addArticle(context, "installation-and-run/software-update.html", "Software Update", 1);

    addArticle(context, "software-configuration/", "Software Configuration");
    addArticle(context, "software-configuration/general-configuration.html", "General Configuration Sequence", 1);
    addArticle(context, "software-configuration/tune-database.html", "Tune up Configuration Database", 1);
    addArticle(context, "software-configuration/using-formulas.html", "Using Formulas", 1);
    addArticle(context, "software-configuration/user-authentication.html", "User Authentication Configuration", 1);
    addArticle(context, "software-configuration/communication-with-devices.html", "Communication with Devices Configuration", 1);
    addArticle(context, "software-configuration/creating-views.html", "Creating Views", 1);

    addArticle(context, "modules/", "Modules");
    addArticle(context, "modules/kp-telegram.html", "Telegram Driver", 1);
    addArticle(context, "modules/mod-auto-control.html", "Automatic Control Module", 1);
    addArticle(context, "modules/mod-db-export.html", "Export to Database Module", 1);
    addArticle(context, "modules/mod-rapid-gate.html", "Rapid Gate Module", 1);
    addArticle(context, "modules/plg-chart-pro.html", "Chart Pro Plugin", 1);
    addArticle(context, "modules/plg-dashboard.html", "Dashboard Plugin", 1);
    addArticle(context, "modules/plg-elastic-report.html", "Elastic Report Plugin", 1);
    addArticle(context, "modules/plg-map.html", "Map Plugin", 1);

    addArticle(context, "use-cases/", "Use Cases");
    addArticle(context, "use-cases/modbus-protocol.html", "Connecting Devices Using Modbus Protocol", 1);
    addArticle(context, "use-cases/opc-standard.html", "Connecting Devices Using OPC Standard", 1);
    addArticle(context, "use-cases/remote-server-management.html", "Managing Remote Server using Agent", 1);
}
