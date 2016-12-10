// Add table of contents in English here
function addContents(context) {
    addArticle(context, "../../", "Home");
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
    addArticle(context, "installation-and-run/run-applications.html", "Run Applications", 1);
    addArticle(context, "installation-and-run/migrate-configuration.html", "Migrate Configuration to New Server", 1);
    addArticle(context, "installation-and-run/software-update.html", "Software Update", 1);

    addArticle(context, "", "Software Configuration");
    addArticle(context, "", "General Configuration Sequence", 1);
    addArticle(context, "", "Tune up Configuration Database", 1);
    addArticle(context, "", "Using Formulas", 1);
    addArticle(context, "", "User Authentication Configuration", 1);
    addArticle(context, "", "Communication with Devices Configuration", 1);
    addArticle(context, "", "Creating Views", 1);

    addArticle(context, "", "Use Cases");
    addArticle(context, "use-cases/modbus-protocol.html", "Connecting Devices Using Modbus Protocol", 1);
    addArticle(context, "use-cases/opc-standard.html", "Connecting Devices Using OPC Standard", 1);

    addArticle(context, "", "Version History");
    addArticle(context, "version-history/scada-history.html", "Rapid SCADA", 1);
    addArticle(context, "", "Modules", 1);
    addArticle(context, "version-history/modules/chart-pro-history.html", "Chart Pro", 2);
    addArticle(context, "version-history/modules/elastic-report-history.html", "Elastic Report", 2);
}
