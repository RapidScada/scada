// Add table of contents in English here
function addContents(context) {
    addArticle(context, "../../../", "Inicio");
    addArticle(context, "software-overview/", "Visión general del software");
    addArticle(context, "software-overview/software-purpose.html", "Propósito y características del software", 1);
    addArticle(context, "software-overview/software-architecture.html", "Arquitectura del software", 1);
    addArticle(context, "software-overview/applications/", "Descripción de las aplicaciones", 1);
    addArticle(context, "software-overview/applications/server-application.html", "Aplicación Server", 2);
    addArticle(context, "software-overview/applications/communicator-application.html", "Aplicación Communicator", 2);
    addArticle(context, "software-overview/applications/webstation-application.html", "Aplicación Webstation", 2);
    addArticle(context, "software-overview/applications/agent-application.html", "Aplicación Agent", 2);
    addArticle(context, "software-overview/applications/administrator-application.html", "Aplicación Administrator", 2);
    addArticle(context, "software-overview/applications/table-editor-application.html", "Aplicación Table Editor", 2);
    addArticle(context, "software-overview/applications/scheme-editor-application.html", "Aplicación Scheme Editor", 2);

    addArticle(context, "installation-and-run/", "Instalación y lanzamiento");
    addArticle(context, "installation-and-run/system-requirements.html", "Requerimientos del sistema", 1);
    addArticle(context, "installation-and-run/software-installation.html", "Instalación del software", 1);
    addArticle(context, "installation-and-run/manual-installation.html", "Instalación manual", 1);
    addArticle(context, "installation-and-run/module-installation.html", "Instalación de módulos adicionales", 1);
    addArticle(context, "installation-and-run/run-applications.html", "Ejecución de aplicaciones", 1);
    addArticle(context, "installation-and-run/migrate-configuration.html", "Migrar la configuración a un nuevo servidor", 1);
    addArticle(context, "installation-and-run/software-update.html", "Actualización del software", 1);
    addArticle(context, "installation-and-run/safety-recommendations.html", "Recomendaciones de seguridad", 1);
     
    addArticle(context, "software-configuration/", "Configuración de software");
    addArticle(context, "software-configuration/configuration-basics.html", "Conceptos básicos de la configuración", 1);
    addArticle(context, "software-configuration/tune-database.html", "Afinación de la base de datos de configuración", 1);
    addArticle(context, "software-configuration/using-formulas.html", "Uso de fórmulas", 1);
    addArticle(context, "software-configuration/user-authentication.html", "Configuración de autenticación del usuario", 1);
    addArticle(context, "software-configuration/communication-with-devices.html", "Configuración de la comunicación con los dispositivos", 1);
    addArticle(context, "software-configuration/creating-views.html", "Creación de vistas", 1);

    addArticle(context, "modules/", "Módulos");
    addArticle(context, "modules/kp-modbus-slave.html", "Controlador de Modbus esclavo", 1);
    addArticle(context, "modules/kp-telegram.html", "Controlador de Telegram", 1);
    addArticle(context, "modules/mod-auto-control.html", "Módulo de Control Automático", 1);
    addArticle(context, "modules/mod-db-export.html", "Módulo de Exportación a Base de Datos", 1);
    addArticle(context, "modules/mod-rapid-gate.html", "Modulo Rapid Gate", 1);
    addArticle(context, "modules/plg-chart-pro.html", "Chart Pro Plugin", 1);
    addArticle(context, "modules/plg-dashboard.html", "Dashboard Plugin", 1);
    addArticle(context, "modules/plg-elastic-report.html", "Elastic Report Plugin", 1);
    addArticle(context, "modules/plg-map.html", "Map Plugin", 1);
    addArticle(context, "modules/plg-notification.html", "Notification Plugin", 1);

    addArticle(context, "additional-applications/", "Additional Applications");
    addArticle(context, "additional-applications/app-auto-report.html", "Aplicación de Informe Automático", 1);

    addArticle(context, "use-cases/", "Use Cases");
    addArticle(context, "use-cases/modbus-protocol.html", "Conexión de dispositivos mediante protocolo Modbus", 1);
    addArticle(context, "use-cases/opc-standard.html", "Conexión de dispositivos utilizando el estándar OPC", 1);
    //addArticle(context, "use-cases/remote-server-management.html", "Managing Remote Server using Agent", 1);

    //addArticle(context, "version-history/", "Version History");
    //addArticle(context, "version-history/scada-history.html", "History of Rapid SCADA", 1);
    //addArticle(context, "version-history/server-history.html", "Server History", 1);
    //addArticle(context, "version-history/server-modules-history.html", "History of Server Modules", 1);
    //addArticle(context, "version-history/communicator-history.html", "Communicator History", 1);
    //addArticle(context, "version-history/communicator-drivers-history.html", "History of Communicator Drivers", 1);
    //addArticle(context, "version-history/administrator-history.html", "Administrator History", 1);
    //addArticle(context, "version-history/webstation-history.html", "Webstation History", 1);
    //addArticle(context, "version-history/webstation-plugins-history.html", "History of Webstation Plugins", 1);
    //addArticle(context, "version-history/table-editor-history.html", "Table Editor History", 1);
    //addArticle(context, "version-history/scheme-editor-history.html", "Scheme Editor History", 1);
}
