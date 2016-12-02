// Add table of contents in Russian here
function addContents(context) {
    addArticle(context, "../../", "Главная");
    addArticle(context, "", "Обзор комплекса");
    addArticle(context, "software-overview/software-purpose.html", "Назначение и характеристики программного комплекса", 1);
    addArticle(context, "software-overview/software-architecture.html", "Архитектура программного комплекса", 1);
    addArticle(context, "", "Описание приложений", 1);
    addArticle(context, "software-overview/server-application.html", "Сервер", 2);
    addArticle(context, "software-overview/communicator-application.html", "Коммуникатор", 2);
    addArticle(context, "software-overview/webstation-application.html", "Вебстанция", 2);
    addArticle(context, "software-overview/administrator-application.html", "Администратор", 2);
    addArticle(context, "software-overview/table-editor-application.html", "Редактор таблиц", 2);
    addArticle(context, "software-overview/scheme-editor-application.html", "Редактор схем", 2);

    addArticle(context, "", "Установка и запуск");
    addArticle(context, "", "System Requirements", 1);
    addArticle(context, "", "Installing Software", 1);
    addArticle(context, "", "Start Applications", 1);
    addArticle(context, "", "Migrating Configuration to New Server", 1);
    addArticle(context, "", "Software Update", 1);

    addArticle(context, "", "Настройка комплекса");
    addArticle(context, "", "General Configuration Sequence", 1);
    addArticle(context, "", "Tune up Configuration Database", 1);
    addArticle(context, "", "Using Formulas", 1);
    addArticle(context, "", "User Authentication Configuration", 1);
    addArticle(context, "", "Communication with Devices Configuration", 1);
    addArticle(context, "", "Creating Views", 1);

    addArticle(context, "", "Сценарии использования");
    addArticle(context, "use-cases/modbus-protocol.html", "Connecting Devices Using Modbus Protocol", 1);
    addArticle(context, "use-cases/opc-standard.html", "Connecting Devices Using OPC Standard", 1);
}
