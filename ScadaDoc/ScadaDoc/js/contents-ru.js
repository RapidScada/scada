// Add table of contents in Russian here
function addContents(context) {
    addArticle(context, "../../", "Главная");
    addArticle(context, "software-overview/", "Обзор комплекса");
    addArticle(context, "software-overview/software-purpose.html", "Назначение и характеристики программного комплекса", 1);
    addArticle(context, "software-overview/software-architecture.html", "Архитектура программного комплекса", 1);
    addArticle(context, "software-overview/applications/", "Описание приложений", 1);
    addArticle(context, "software-overview/applications/server-application.html", "Приложение Сервер", 2);
    addArticle(context, "software-overview/applications/communicator-application.html", "Приложение Коммуникатор", 2);
    addArticle(context, "software-overview/applications/webstation-application.html", "Приложение Вебстанция", 2);
    addArticle(context, "software-overview/applications/administrator-application.html", "Приложение Администратор", 2);
    addArticle(context, "software-overview/applications/table-editor-application.html", "Приложение Редактор таблиц", 2);
    addArticle(context, "software-overview/applications/scheme-editor-application.html", "Приложение Редактор схем", 2);

    addArticle(context, "installation-and-run/", "Установка и запуск");
    addArticle(context, "installation-and-run/system-requirements.html", "Системные требования", 1);
    addArticle(context, "installation-and-run/software-installation.html", "Установка программного комплекса", 1);
    addArticle(context, "installation-and-run/manual-installation.html", "Установка вручную", 1);
    addArticle(context, "installation-and-run/run-applications.html", "Запуск приложений", 1);
    addArticle(context, "installation-and-run/migrate-configuration.html", "Перенос конфигурации на новый сервер", 1);
    addArticle(context, "installation-and-run/software-update.html", "Обновление программного комплекса", 1);

    addArticle(context, "software-configuration/", "Настройка комплекса");
    addArticle(context, "software-configuration/general-configuration.html", "Общая последовательность настройки", 1);
    addArticle(context, "software-configuration/tune-database.html", "Создание базы конфигурации", 1);
    addArticle(context, "software-configuration/using-formulas.html", "Использование формул", 1);
    addArticle(context, "software-configuration/user-authentication.html", "Настройка аутентификации пользователей", 1);
    addArticle(context, "software-configuration/communication-with-devices.html", "Настройка обмена данными с устройствами", 1);
    addArticle(context, "software-configuration/creating-views.html", "Создание представлений", 1);

    addArticle(context, "", "Модули");
    addArticle(context, "modules/mod-db-export.html", "Модуль экспорта в БД", 1);

    addArticle(context, "", "Сценарии использования");
    addArticle(context, "use-cases/modbus-protocol.html", "Подключение устройств по протоколу Modbus", 1);
    addArticle(context, "use-cases/opc-standard.html", "Подключение устройств с использованием стандарта OPC", 1);

    /*addArticle(context, "", "История версий");
    addArticle(context, "version-history/scada-history.html", "Rapid SCADA", 1);
    addArticle(context, "", "Модули", 1);
    addArticle(context, "version-history/modules/chart-pro-history.html", "Графики Про", 2);
    addArticle(context, "version-history/modules/elastic-report-history.html", "Гибкий отчёт", 2);*/
}
