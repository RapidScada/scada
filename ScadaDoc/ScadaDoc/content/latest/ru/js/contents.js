// Add table of contents in Russian here
function addContents(context) {
    addArticle(context, "../../../", "Главная");
    addArticle(context, "software-overview/", "Обзор комплекса");
    addArticle(context, "software-overview/software-purpose.html", "Назначение и характеристики программного комплекса", 1);
    addArticle(context, "software-overview/software-architecture.html", "Архитектура программного комплекса", 1);
    addArticle(context, "software-overview/applications/", "Описание приложений", 1);
    addArticle(context, "software-overview/applications/server-application.html", "Приложение Сервер", 2);
    addArticle(context, "software-overview/applications/communicator-application.html", "Приложение Коммуникатор", 2);
    addArticle(context, "software-overview/applications/webstation-application.html", "Приложение Вебстанция", 2);
    addArticle(context, "software-overview/applications/agent-application.html", "Приложение Агент", 2);
    addArticle(context, "software-overview/applications/administrator-application.html", "Приложение Администратор", 2);
    addArticle(context, "software-overview/applications/table-editor-application.html", "Приложение Редактор таблиц", 2);
    addArticle(context, "software-overview/applications/scheme-editor-application.html", "Приложение Редактор схем", 2);
    addArticle(context, "software-overview/roadmap.html", "Дорожная карта", 1);

    addArticle(context, "installation-and-run/", "Установка и запуск");
    addArticle(context, "installation-and-run/system-requirements.html", "Системные требования", 1);
    addArticle(context, "installation-and-run/software-installation.html", "Установка программного комплекса", 1);
    addArticle(context, "installation-and-run/manual-installation.html", "Установка вручную", 1);
    addArticle(context, "installation-and-run/module-installation.html", "Установка дополнительных модулей", 1);
    addArticle(context, "installation-and-run/run-applications.html", "Запуск приложений", 1);
    addArticle(context, "installation-and-run/migrate-configuration.html", "Перенос конфигурации на новый сервер", 1);
    addArticle(context, "installation-and-run/software-update.html", "Обновление программного комплекса", 1);
    addArticle(context, "installation-and-run/safety-recommendations.html", "Рекомендации по безопасности", 1);

    addArticle(context, "software-configuration/", "Настройка комплекса");
    addArticle(context, "software-configuration/configuration-basics.html", "Основы настройки", 1);
    addArticle(context, "software-configuration/tune-database.html", "Настройка базы конфигурации", 1);
    addArticle(context, "software-configuration/using-formulas.html", "Использование формул", 1);
    addArticle(context, "software-configuration/user-authentication.html", "Настройка аутентификации пользователей", 1);
    addArticle(context, "software-configuration/communication-with-devices.html", "Настройка обмена данными с устройствами", 1);
    addArticle(context, "software-configuration/creating-views.html", "Создание представлений", 1);

    addArticle(context, "modules/", "Модули");
    addArticle(context, "modules/kp-db-import.html", "Драйвер импорта из БД", 1);
    addArticle(context, "modules/kp-modbus-slave.html", "Драйвер Modbus Slave", 1);
    addArticle(context, "modules/kp-telegram.html", "Драйвер Telegram", 1);
    addArticle(context, "modules/mod-auto-control.html", "Модуль автоматического управления", 1);
    addArticle(context, "modules/mod-db-export.html", "Модуль экспорта в БД", 1);
    addArticle(context, "modules/mod-rapid-gate.html", "Модуль Быстрый шлюз", 1);
    addArticle(context, "modules/plg-chart-pro.html", "Плагин Графики Про", 1);
    addArticle(context, "modules/plg-dashboard.html", "Плагин Дэшборды", 1);
    addArticle(context, "modules/plg-elastic-report.html", "Плагин Гибкий отчёт", 1);
    addArticle(context, "modules/plg-map.html", "Плагин Карты", 1);
    addArticle(context, "modules/plg-notification.html", "Плагин Уведомления", 1);

    addArticle(context, "additional-applications/", "Дополнительные приложения");
    addArticle(context, "additional-applications/app-auto-report.html", "Приложение Автоотчёт", 1);

    addArticle(context, "use-cases/", "Сценарии использования");
    addArticle(context, "use-cases/modbus-protocol.html", "Подключение устройств по протоколу Modbus", 1);
    addArticle(context, "use-cases/opc-standard.html", "Подключение устройств с использованием стандарта OPC", 1);

    addArticle(context, "version-history/", "История версий");
    addArticle(context, "version-history/scada-history.html", "История Rapid SCADA", 1);
    addArticle(context, "version-history/server-history.html", "История приложения Сервер", 1);
    addArticle(context, "version-history/server-modules-history.html", "История модулей Сервера", 1);
    addArticle(context, "version-history/communicator-history.html", "История приложения Коммуникатор", 1);
    addArticle(context, "version-history/communicator-drivers-history.html", "История драйверов Коммуникатора", 1);
    addArticle(context, "version-history/webstation-history.html", "История приложения Вебстанция", 1);
    addArticle(context, "version-history/webstation-plugins-history.html", "История плагинов Вебстанции", 1);
    addArticle(context, "version-history/agent-history.html", "История приложения Агент", 1);
    addArticle(context, "version-history/administrator-history.html", "История приложения Администратор", 1);
    addArticle(context, "version-history/table-editor-history.html", "История приложения Редактор таблиц", 1);
    addArticle(context, "version-history/scheme-editor-history.html", "История приложения Редактор схем", 1);
}
