using System.Collections.Generic;

namespace Setup_RadioPlayer
{
    public static class LanguageManager
    {
        public enum Language
        {
            Ukrainian,
            English,
            Italian
        }

        private static Language currentLanguage = Language.Ukrainian;
        private static Dictionary<string, Dictionary<Language, string>> translations = new Dictionary<string, Dictionary<Language, string>>()
        {
            ["AppTitle"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Установник Old RadioPlayer",
                [Language.English] = "Old RadioPlayer Installer",
                [Language.Italian] = "Programma di installazione Old RadioPlayer"
            },
            ["WelcomeTitle"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Ласкаво просимо до установника Old RadioPlayer!",
                [Language.English] = "Welcome to Old RadioPlayer Installer!",
                [Language.Italian] = "Benvenuto nel programma di installazione di Old RadioPlayer!"
            },
            ["WelcomeDescription"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Ця програма встановить Old RadioPlayer на ваш комп'ютер.\nНова версія RadioPlayer знаходиться в розробці.\nНатисніть 'Далі' для продовження.",
                [Language.English] = "This program will install Old RadioPlayer on your computer.\nNew version of RadioPlayer is under development.\nClick 'Next' to continue.",
                [Language.Italian] = "Questo programma installerà Old RadioPlayer sul tuo computer.\nLa nuova versione di RadioPlayer è in fase di sviluppo.\nFai clic su 'Avanti' per continuare."
            },
            ["LicenseTitle"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Ліцензійна угода",
                [Language.English] = "License Agreement",
                [Language.Italian] = "Contratto di licenza"
            },
            ["LicenseAgree"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Я прочитав(ла) та погоджуюся з умовами ліцензійної угоди",
                [Language.English] = "I have read and agree to the terms of the license agreement",
                [Language.Italian] = "Ho letto e accetto i termini del contratto di licenza"
            },
            ["InstallPathTitle"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Програма встановлення встановить Old RadioPlayer у наступну папку.\nЩоб продовжити, натисніть 'Далі'. Якщо ви хочете вибрати іншу папку, виберіть 'Огляд'.",
                [Language.English] = "The installer will install Old RadioPlayer to the following folder.\nTo continue, click 'Next'. If you want to select a different folder, choose 'Browse'.",
                [Language.Italian] = "Il programma di installazione installerà Old RadioPlayer nella seguente cartella.\nPer continuare, fare clic su 'Avanti'. Se si desidera selezionare una cartella diversa, scegliere 'Sfoglia'."
            },
            ["OptionsTitle"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Оберіть додаткові опції установки:",
                [Language.English] = "Select additional installation options:",
                [Language.Italian] = "Seleziona le opzioni di installazione aggiuntive:"
            },
            ["ProgressTitle"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Виконується установка, будь ласка зачекайте...",
                [Language.English] = "Installation in progress, please wait...",
                [Language.Italian] = "Installazione in corso, attendere prego..."
            },
            ["FinishTitle"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Дякуємо за встановлення Old RadioPlayer!",
                [Language.English] = "Thank you for installing Old RadioPlayer!",
                [Language.Italian] = "Grazie per aver installato Old RadioPlayer!"
            },
            ["BtnNext"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Далі",
                [Language.English] = "Next",
                [Language.Italian] = "Avanti"
            },
            ["BtnBack"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Назад",
                [Language.English] = "Back",
                [Language.Italian] = "Indietro"
            },
            ["BtnInstall"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Встановити",
                [Language.English] = "Install",
                [Language.Italian] = "Installa"
            },
            ["BtnFinish"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Готово",
                [Language.English] = "Finish",
                [Language.Italian] = "Fine"
            },
            ["BtnBrowse"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Огляд",
                [Language.English] = "Browse",
                [Language.Italian] = "Sfoglia"
            },
            ["LaunchAfterInstall"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Запустити програму після встановлення",
                [Language.English] = "Launch the program after installation",
                [Language.Italian] = "Avvia il programma dopo l'installazione"
            },
            ["VisitWebsite"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Відвідати сайт",
                [Language.English] = "Visit website",
                [Language.Italian] = "Visita il sito web"
            },
            ["CreateDesktopShortcut"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Створити ярлик на робочому столі",
                [Language.English] = "Create desktop shortcut",
                [Language.Italian] = "Crea collegamento sul desktop"
            },
            ["CreateStartMenuShortcut"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Створити ярлик в меню Пуск",
                [Language.English] = "Create Start Menu shortcut",
                [Language.Italian] = "Crea collegamento nel menu Start"
            },
            ["RunAtStartup"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Запускати програму разом з Windows",
                [Language.English] = "Run the program with Windows",
                [Language.Italian] = "Avvia il programma con Windows"
            },
            ["LaunchApp"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Запустити програму",
                [Language.English] = "Launch the program",
                [Language.Italian] = "Avvia il programma"
            },
            ["SupportEmail"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Підтримка: support@deepradio.cloud",
                [Language.English] = "Support: support@deepradio.cloud",
                [Language.Italian] = "Supporto: support@deepradio.cloud"
            },
            ["Website"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Сайт: deepradio.cloud",
                [Language.English] = "Website: deepradio.cloud",
                [Language.Italian] = "Sito web: deepradio.cloud"
            },
            ["BetaMessage"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Це бета-версія програми. Можливі помилки та недоробки.\nЯкщо ви виявили проблему, повідомте нам на адресу support@deepradio.cloud",
                [Language.English] = "This is a beta version of the program. Errors and bugs are possible.\nIf you have found a problem, please contact us at support@deepradio.cloud",
                [Language.Italian] = "Questa è una versione beta del programma. Errori e bug sono possibili.\nSe hai riscontrato un problema, contattaci all'indirizzo support@deepradio.cloud"
            },
            ["CancelConfirm"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Ви дійсно хочете скасувати установку?",
                [Language.English] = "Do you really want to cancel the installation?",
                [Language.Italian] = "Vuoi davvero annullare l'installazione?"
            },
            ["Confirmation"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Підтвердження",
                [Language.English] = "Confirmation",
                [Language.Italian] = "Conferma"
            },
            ["Warning"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Попередження",
                [Language.English] = "Warning",
                [Language.Italian] = "Avvertimento"
            },
            ["Error"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Помилка",
                [Language.English] = "Error",
                [Language.Italian] = "Errore"
            },
            ["Information"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Інформація",
                [Language.English] = "Information",
                [Language.Italian] = "Informazione"
            },
            ["MustAcceptLicense"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Ви повинні прийняти ліцензійну угоду для продовження.",
                [Language.English] = "You must accept the license agreement to continue.",
                [Language.Italian] = "È necessario accettare il contratto di licenza per continuare."
            },
            ["InvalidPath"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Будь ласка, оберіть коректний шлях установки.",
                [Language.English] = "Please select a valid installation path.",
                [Language.Italian] = "Seleziona un percorso di installazione valido."
            },
            ["FolderExists"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Папка вже існує. Ви хочете перезаписати її?",
                [Language.English] = "The folder already exists. Do you want to overwrite it?",
                [Language.Italian] = "La cartella esiste già. Vuoi sovrascriverla?"
            },
            ["NotEnoughSpace"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Недостатньо вільного місця на диску.",
                [Language.English] = "Not enough free disk space.",
                [Language.Italian] = "Spazio su disco insufficiente."
            },
            ["InstallError"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Під час установки сталася помилка: ",
                [Language.English] = "An error occurred during installation: ",
                [Language.Italian] = "Si è verificato un errore durante l'installazione: "
            },
            ["Step1"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Крок 1/4: Розпакування файлів...",
                [Language.English] = "Step 1/4: Extracting files...",
                [Language.Italian] = "Passaggio 1/4: Estrazione file..."
            },
            ["Step2"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Крок 2/4: Перевірка цілісності файлів...",
                [Language.English] = "Step 2/4: Verifying file integrity...",
                [Language.Italian] = "Passaggio 2/4: Verifica dell'integrità dei file..."
            },
            ["Step3"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Крок 3/4: Створення ярликів...",
                [Language.English] = "Step 3/4: Creating shortcuts...",
                [Language.Italian] = "Passaggio 3/4: Creazione di scorciatoie..."
            },
            ["Step4"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Крок 4/4: Додавання в автозапуск...",
                [Language.English] = "Step 4/4: Adding to startup...",
                [Language.Italian] = "Passaggio 4/4: Aggiunta all'avvio automatico..."
            },
            ["Completing"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Завершення установки...",
                [Language.English] = "Completing installation...",
                [Language.Italian] = "Completamento dell'installazione..."
            },
            ["InstallComplete"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Установка завершена успішно!",
                [Language.English] = "Installation completed successfully!",
                [Language.Italian] = "Installazione completata con successo!"
            },
            ["RequiredSpace"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Необхідно місця",
                [Language.English] = "Required space",
                [Language.Italian] = "Spazio richiesto"
            },
            ["AvailableSpace"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Доступно",
                [Language.English] = "Available",
                [Language.Italian] = "Disponibile"
            },
            ["LanguageLabel"] = new Dictionary<Language, string>
            {
                [Language.Ukrainian] = "Мова / Language / Lingua:",
                [Language.English] = "Language / Мова / Lingua:",
                [Language.Italian] = "Lingua / Language / Мова:"
            }
        };

        public static Language CurrentLanguage
        {
            get { return currentLanguage; }
            set { currentLanguage = value; }
        }

        public static string GetString(string key)
        {
            if (translations.ContainsKey(key) && translations[key].ContainsKey(currentLanguage))
            {
                return translations[key][currentLanguage];
            }
            return key;
        }

        public static string GetLanguageName(Language lang)
        {
            switch (lang)
            {
                case Language.Ukrainian:
                    return "Українська";
                case Language.English:
                    return "English";
                case Language.Italian:
                    return "Italiano";
                default:
                    return "Unknown";
            }
        }
    }
}
