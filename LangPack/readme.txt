Language Installation
---------------------
1. Choose the language and open the appropriate folder of the language pack.
2. Copy the language files from the folder SCADA of the language pack to the installation directory of Rapid SCADA maintaining the hierarchy of the directories. 
3. Run the Administrator application.
4. Go to Tools -> Language and type the culture name, e.g. es-ES, and click OK button.
5. Restart Rapid SCADA applications to apply changes.


How to Localize UI to Any Language
----------------------------------
Most of Rapid SCADA applications contain special localization files. Usually they are located in a Lang folder:
C:\SCADA\ScadaAgent\Lang
C:\SCADA\ScadaAdmin\Lang
C:\SCADA\ScadaComm\Lang
C:\SCADA\ScadaSchemeEditor\Lang
C:\SCADA\ScadaServer\Lang
C:\SCADA\ScadaTableEditor\Lang
C:\SCADA\ScadaWeb\lang

To add a new language support, create copies of *.en-GB.xml files and give the file names according to your culture. In this example "en" means English language and "GB" is Great Britain.
Using your favorite text editor (Notepad++ is OK) translate the phrases in the created xml files. Then open the Administrator application, go to Tools -> Language menu and enter your localization name, for example, es-ES

Keep unchanged in XML:
* Numbers in curly braces, for example, {0}
* Text prefixed with "&", for example, &quot; &gt;
