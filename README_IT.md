# Programma di installazione Old RadioPlayer

 

![Version](https://img.shields.io/badge/version-2.0-blue.svg)

![Status](https://img.shields.io/badge/status-production-green.svg)

![Language](https://img.shields.io/badge/language-C%23-blue.svg)

![Multi-Language](https://img.shields.io/badge/lingue-EN%20%7C%20UA%20%7C%20IT-brightgreen.svg)

 

## 📋 Panoramica

 

Un programma di installazione moderno e professionale per Old RadioPlayer, realizzato in C# utilizzando Windows Forms. Il programma di installazione presenta un'elegante interfaccia multilingue (ucraino, inglese, italiano) e tutte le funzioni necessarie per un'installazione del software sicura e conveniente.

 

> **Nota**: La nuova versione di RadioPlayer è attualmente in fase di sviluppo attivo. Questo programma di installazione è per la versione legacy "Old RadioPlayer".

 

## ✨ Caratteristiche

 

- 🌍 **Supporto multilingue** - Interfacce in ucraino, inglese e italiano

- 🎨 **Design moderno** - Bordi sfumati, pulsanti animati, angoli arrotondati

- 🔐 **Sicurezza** - Verifica dei diritti di amministratore, protezione dagli attacchi Zip Slip

- 📊 **Progresso dettagliato** - Indicatore di installazione passo-passo con percentuali

- 🗂️ **Supporto OneDrive** - Gestione intelligente dei collegamenti per OneDrive Desktop

- 📝 **Registrazione dettagliata** - Tutte le azioni vengono registrate in install.log

- ↩️ **Ripristino dell'installazione** - Ripristino automatico delle modifiche in caso di errore

- 🗑️ **Programma di disinstallazione** - Supporto completo per la disinstallazione con pulizia del registro

 

## 🔧 Caratteristiche tecniche

 

### Componenti

 

1. **LanguageManager.cs** - Sistema multilingue

   - Supporto per 3 lingue

   - Cambio dinamico della lingua

   - Copertura completa della traduzione

 

2. **MainForm.cs** - Modulo principale del programma di installazione

   - Interfaccia multi-pannello

   - Installazione passo-passo

   - Convalida del percorso

   - Verifica dello spazio su disco

 

3. **ShortcutManager.cs** - Gestore di collegamenti

   - Supporto OneDrive Desktop

   - Rilevamento di più percorsi Desktop

   - Verifica basata sul registro

   - Meccanismi di fallback

 

4. **RegistryManager.cs** - Gestore del registro

   - Aggiunta all'avvio automatico

   - Verifica dell'esistenza dei file

   - Rimozione sicura delle voci del registro

 

5. **FileManager.cs** - Gestore di file

   - Protezione Zip Slip

   - Monitoraggio del progresso dell'estrazione

   - Convalida del percorso

   - Nomi di file temporanei univoci

 

6. **UninstallManager.cs** - Programma di disinstallazione

   - Rimozione completa dei file

   - Pulizia dei collegamenti

   - Pulizia del registro

   - Rimozione della cartella del menu Start

 

7. **CustomButton.cs** - Pulsanti personalizzati

   - Animazioni fluide

   - Gradienti e ombre

   - Design moderno

   - Effetti hover

 

## 🚀 Utilizzo

 

### Requisiti di sistema

- Windows 7 o versioni successive

- .NET Framework 4.8 o versioni successive

- Diritti di amministratore

- Minimo 50 MB di spazio libero

 

### Processo di installazione

 

1. **Avvio**

   - Fare doppio clic su Setup.exe

   - Concedere i diritti di amministratore

 

2. **Selezione della lingua**

   - Scegliere la lingua preferita

   - Disponibili: ucraino, inglese, italiano

 

3. **Contratto di licenza**

   - Leggere e accettare il contratto

 

4. **Selezione del percorso**

   - Predefinito: `C:\Program Files\RadioPlayer`

   - Opzione per scegliere un percorso personalizzato

   - Verifica dello spazio libero

 

5. **Opzioni di installazione**

   - ☑️ Avviare dopo l'installazione

   - ☐ Visitare il sito web

   - ☐ Creare collegamento sul desktop

   - ☐ Creare collegamento nel menu Start

   - ☐ Eseguire all'avvio di Windows

 

6. **Installazione**

   - Estrazione dei file (0-70%)

   - Verifica dell'integrità dei file (70-75%)

   - Creazione di collegamenti (75-85%)

   - Configurazione dell'avvio (85-90%)

   - Creazione del programma di disinstallazione (90-98%)

   - Completamento (98-100%)

 

7. **Fine**

   - Avvio opzionale del programma

   - Visita opzionale del sito web

 

## 📁 Struttura dei file

 

```

Installer-DeepRadio/
│
├── MainForm.cs              # Modulo principale
├── LanguageManager.cs       # Sistema multilingue
├── ShortcutManager.cs       # Gestore di collegamenti
├── RegistryManager.cs       # Gestore del registro
├── FileManager.cs           # Gestore di file
├── UninstallManager.cs      # Programma di disinstallazione
├── CustomButton.cs          # Pulsanti personalizzati
├── BetaButton.cs            # Indicatore beta
│
├── license_ua.txt           # Licenza ucraina
├── license_en.txt           # Licenza inglese
├── license_it.txt           # Licenza italiana
│
├── README_EN.md             # Documentazione inglese
├── README_IT.md             # Documentazione italiana
│
└── Properties/
    └── ...                  # Risorse e impostazioni

```

 

## 🛡️ Sicurezza

 

### Protezioni implementate:

 

1. **Diritti di amministratore** - Verifica e richiesta dei diritti

2. **Protezione Zip Slip** - Convalida del percorso durante l'estrazione

3. **Verifica del percorso** - Directory di sistema vietate

4. **Verifica dello spazio** - Spazio su disco sufficiente

5. **Convalida dei file** - Verifica dell'esistenza del file exe

6. **Ripristino sicuro** - Rimozione solo da Program Files

 

## 🌐 Supporto

 

- **Email:** support@deepradio.cloud

- **Sito web:** https://deepradio.cloud

 

## 📝 Licenza

 

Questo programma di installazione è sviluppato per Old RadioPlayer.

Tutti i diritti riservati © 2025 DeepRadio

 

## 🎉 Ringraziamenti

 

Grazie per aver utilizzato il programma di installazione Old RadioPlayer!

 

---

 

**Versione:** 2.0 (Modernizzato)

**Data:** Novembre 2025

**Developed & engineered by Vados2343**
