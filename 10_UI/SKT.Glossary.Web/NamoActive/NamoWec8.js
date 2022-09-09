document.onreadystatechange=function()
{
 if (document.readyState == 'complete')
 {
      if (document.all['divShowInstall'])
        document.all['divShowInstall'].style.visibility = 'hidden';
  }
}

var strScripts1   ="<OBJECT ID='Wec' CLASSID='CLSID:FDA7CDA8-66D0-49A6-B275-3E7A80C1A9BA' WIDTH='723' HEIGHT='525' CodeBase='/NamoActive/NamoWec8.cab#Version=8,0,0,10'>";
strScripts1      +="<PARAM NAME='UserLang' VALUE='kor'>";
strScripts1      +="<PARAM NAME='InitFileURL' VALUE='/NamoActive/As8Init.xml'>";
strScripts1      +="<PARAM NAME='InitFileVer' VALUE='-9999'>";
strScripts1      +="<PARAM NAME='InitFileWaitTime' VALUE='3000'>";
strScripts1      +="<PARAM NAME='EditorAutoSaveInterval' VALUE='10'>";
strScripts1      +="<PARAM NAME='EditorBackupOnOff' VALUE='True'>";
strScripts1      +="<PARAM NAME='InstallSourceURL' VALUE='http://comp.namo.co.kr/as8/AS8_update/'>";
strScripts1      +="<param name='TemplateIniURL' value='http://localtikle.sktelecom.com/NamoActive/Template/template.txt'>";
strScripts1      +="<param name='wmode' value='transparent'>";
strScripts1      +="</OBJECT>";

document.write(strScripts1);