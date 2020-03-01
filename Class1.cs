using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FilFunksjoner
{
    public class Class1
    {

        /// <summary>
        /// Fjerner førstemappen i fil. Returnerer den endrede strengen.
        /// </summary>
        /// <param name="mappe">Path til mappe.</param>
        /// <returns>Mappenavnet uten den første mappen.</returns>
        public static string FjernFørsteMappeFraMappeStreng(string mappe)
        {
            for (int i = 1; i < mappe.Length; i++) if (mappe[i] == '\\') return mappe.Remove(0, i + 1);
            return mappe;
        }

        /// <summary>
        /// Fjerner sistemappen i fil. Returnerer den endrede strengen.
        /// </summary>
        /// <param name="mappe">Path til mappe.</param>
        /// <returns>Mappenavnet uten den siste mappen.</returns>
        public static string FjernSisteMappeFraMappeStreng(string mappe)
        {
            for (int i = mappe.Length - 2; i > 0; i--) if (mappe[i] == '\\') return mappe.Remove(i + 1); //mappe.Length - 2 i tilfelle mappe ender på \. mappe.Remove(i + 1) for å beholde \.
            return mappe;
        }

        /// <summary>
        /// Henter sistemappen i fil. Returnerer denne som streng, uten path.
        /// </summary>
        /// <param name="mappe">Path til mappe.</param>
        /// <returns>Mappenavnet til bare siste mappen, uten path.</returns>
        public static string HentSisteMappeFraMappeStreng(string mappe)
        {
            if (mappe[mappe.Length - 1] == '\\') mappe = mappe.Remove(mappe.Length - 1);
            for (int i = mappe.Length - 2; i > 0; i--) if (mappe[i] == '\\') return mappe.Remove(0, i + 1); //mappe.Length - 2 i tilfelle mappe ender på \. mappe.Remove(i + 1) for å beholde \.
            return mappe;
        }

        /// <summary>
        /// Fjerner fildelen fra filens fulle filnavn.
        /// </summary>
        /// <param name="fil">Fullt filnavn.</param>
        /// <returns>Filens Path.</returns>
        public static string FjernFilFraMappeStreng(string fil)
        {
            for (int i = fil.Length - 1; i > 0; i--) if (fil[i] == '\\')
                {
                    fil = fil.Remove(i);
                    if (fil[fil.Length - 1] != '\\') fil += '\\';
                    return fil;
                }
            return fil;
        }

        /// <summary>
        /// Henter fildelen fra filens fulle filnavn.
        /// </summary>
        /// <param name="fil">Fullt filnavn.</param>
        /// <returns>Filens Path.</returns>
        public static string HentFilFraMappeStreng(string fil)
        {
            for (int i = fil.Length - 1; i > 0; i--) if (fil[i] == '\\')
                {
                    fil = fil.Remove(0, i + 1);
                    return fil;
                }
            return fil;
        }

        /// <summary>
        /// Sletter alle tomme undermapper, og så mappe hvis tom.
        /// </summary>
        /// <param name="mappe">Mappen som skal slettes.</param>
        public static void MappeSlettTomme(string mappe)
        {
            ;
            foreach (string mappeSlett in Directory.GetDirectories(mappe)) MappeSlettTomme(mappeSlett);
            try
            {
                Directory.Delete(mappe); //Sletter tomme mapper.
            }
            catch (System.IO.IOException) { }
        }

        /// <summary>
        /// Lager en liste over alle undermapper til mappen mappeStr.
        /// </summary>
        /// <param name="mappeLiStr">Listen over undermapper til mappeStr</param>
        /// <param name="mappeStr">Mappen som skal sjekkes.</param>
        private static void MapperUnderLike_LagLi(ref List<string> mappeLiStr, string mappeStr)
        {
            DirectoryInfo mappe = new DirectoryInfo(mappeStr);
            DirectoryInfo[] mappeLi = mappe.GetDirectories();
            foreach (DirectoryInfo mappeUnder in mappeLi)
            {
                mappeLiStr.Add(mappeUnder.Name);
                MapperUnderLike_LagLi(ref mappeLiStr, mappeUnder.FullName);
            }
        }

        /// <summary>
        /// Returnerer true hvis mappe1 er lik mappe2, eller hvis alle undermappene deres er like.
        /// </summary>
        /// <param name="mappe1">Full path til mappen som skal sammenlignes med mappen mappe2</param>
        /// <param name="mappe2">Full path til mappen som skal sammenlignes med mappen mappe1</param>
        /// <returns>Returnerer true hvis mappe1 er lik mappe2, eller hvis alle undermappene deres er like.</returns>
        public static bool MapperUnderLike(string mappe1, string mappe2)
        {
            if (mappe1.Equals(mappe2, StringComparison.Ordinal)) return true;
            if (mappe1[mappe1.Length - 1] != '\\') mappe1 += '\\';
            if (mappe2[mappe2.Length - 1] != '\\') mappe2 += '\\';
            List<string> mappe1Li = new List<string>();
            List<string> mappe2Li = new List<string>();
            MapperUnderLike_LagLi(ref mappe1Li, mappe1);
            MapperUnderLike_LagLi(ref mappe2Li, mappe2);
            if (mappe1Li.Count != mappe2Li.Count) return false;
            for (int i = 0; i < mappe1Li.Count; i++) if (!mappe1Li[i].Equals(mappe2Li[i], StringComparison.Ordinal)) return false;
            return true;
        }

        /// <summary>
        /// Flytter alt i mappen mappeTilStr til mappen mappeFraStr. Hvis flyttTilLikMappe er false, så vil den forandre navnet på mapper som har samme navn. Ellers vil den slå sammen mapper med likt navn.
        /// </summary>
        /// <param name="mappeTilStr"></param>
        /// <param name="mappeFraStr"></param>
        /// <param name="flyttTilLikMappe"></param>
        /// <returns></returns>
        public static bool MappeFlytt(string mappeTilStr, string mappeFraStr, bool flyttTilLikMappe)
        {
            if (mappeTilStr[mappeTilStr.Length - 1] != '\\') mappeTilStr += '\\';
            if (mappeFraStr[mappeFraStr.Length - 1] != '\\') mappeFraStr += '\\';
            DirectoryInfo mappeTil = new DirectoryInfo(mappeTilStr);
            DirectoryInfo mappeFra = new DirectoryInfo(mappeFraStr);
            if (mappeTilStr.Contains(mappeFraStr)) return false;
            if (!flyttTilLikMappe && mappeTil.Name == mappeFra.Name)
            {
                int teller = 2;
                string mappeTemp = mappeTil.FullName + '_' + teller;
                while (Directory.Exists(mappeTemp)) mappeTemp = mappeTil.FullName + '_' + (++teller);
                mappeFra.MoveTo(mappeTemp);
            }
            if (!mappeTil.Exists) mappeTil.Create();
            foreach (FileInfo fil in mappeFra.GetFiles()) fil.MoveTo(mappeTil.FullName + fil.Name);
            DirectoryInfo[] mappeLi = mappeFra.GetDirectories();
            foreach (DirectoryInfo mappeUnder in mappeLi)
            {
                //    string mappeTemp2 = mappeTil.FullName + mappeUnder.Name;
                //    if(mappeTemp2[mappeTemp2.Length - 1] != '\\') mappeTemp2 += '\\';
                if (Directory.Exists(mappeTil.FullName + mappeUnder.Name))
                {
                    if (flyttTilLikMappe) MappeFlytt(mappeTil + mappeUnder.Name, mappeUnder.FullName, flyttTilLikMappe);
                    else
                    {
                        int teller = 2;
                        string mappeTemp = mappeTil.FullName + mappeUnder.Name + '_' + teller;
                        while (Directory.Exists(mappeTemp)) mappeTemp = mappeTil.FullName + mappeUnder.Name + '_' + (++teller);
                        mappeUnder.MoveTo(mappeTemp);
                    }
                }
                else mappeUnder.MoveTo(mappeTil.FullName);
                try
                {
                    mappeUnder.Delete(); //Sletter tomme mapper.
                }
                catch (System.IO.IOException) { }
            }
            try
            {
                mappeFra.Delete(); //Sletter tomme mapper.
            }
            catch (System.IO.IOException) { }
            return true;
        }

        /// <summary>
        /// Sjekker om en fil finnes.
        /// </summary>
        /// <param name="fil">Full path med navn til filen.</param>
        /// <param name="feilMeldingTilKonsollDefFalse">Valgfri. Skriver feilmelding til konsollvinduet hvis filen ikke finnes. Default er false.</param>
        /// <param name="feilMelding">Valgfri. Tar vare på feilmeldingene.</param>
        /// <returns>true hvis filen finnes.</returns>
        public static bool FilFinnes(string fil, bool feilMeldingTilKonsollDefFalse, ref StringBuilder feilMelding)
        {
            try
            {
                if (!System.IO.File.Exists(fil))
                {
                    if (feilMeldingTilKonsollDefFalse)
                    {
                        Console.WriteLine("Finner ikke filen \"" + fil + "\".");
                        feilMelding.AppendLine("Finner ikke filen \"" + fil + "\".");
                    }
                    return false;
                }
            }
            catch
            {
                if (feilMeldingTilKonsollDefFalse)
                {
                    Console.WriteLine("Ugyldig filnavn \"" + fil + "\". Greier ikke å sjekke om filen finnes.");
                    feilMelding.AppendLine("Ugyldig filnavn \"" + fil + "\". Greier ikke å sjekke om filen finnes.");
                }
            }
            return true;
        }

        /// <summary>
        /// Sjekker om en fil finnes.
        /// </summary>
        /// <param name="fil">Full path med navn til filen.</param>
        /// <returns>true hvis filen finnes.</returns>
        public static bool FilFinnes(string fil)
        {
            StringBuilder tmp = new StringBuilder();
            return FilFinnes(fil, false, ref tmp);
        }

        /// <summary>
        /// Tar kopi av en fil. Den nye filen vil ende med versjonsnummer og .bkp endelse (t.txt -> t.txt_v1.bkp).
        /// Versjonsnummeret oppdateres i forhold til tidligere backupkopier.
        /// </summary>
        /// <param name="filPath">Path'en til mappen som filen ligger i.</param>
        /// <param name="filNavn">Navnet på filen man vil kopiere.</param>
        /// <param name="backupMappePath">Valgfri. Path'en til mappen som kopien skal havne i. Default er filPath.</param>
        static void FilBackup(string filPath, string filNavn, string backupMappePath)
        {
            int v = 1;
            filPath += "\\";
            backupMappePath += "\\";
            string nyFilNavn = filNavn + "_" + v + ".bkp";
            if (System.IO.File.Exists(backupMappePath + nyFilNavn))
            {
                while (System.IO.File.Exists(backupMappePath + filNavn + "_" + ++v + ".bkp")) ;
                nyFilNavn = filNavn + "_" + v + ".bkp";
            }
            System.IO.File.Copy(filPath + filNavn, backupMappePath + nyFilNavn);
        }

        /// <summary>
        /// Tar kopi av en fil. Den nye filen vil ende med versjonsnummer og .bkp endelse (t.txt -> t.txt_v1.bkp), og lagres i samme mappe som originalen.
        /// Versjonsnummeret oppdateres i forhold til tidligere backupkopier.
        /// </summary>
        /// <param name="filPath">Path'en til mappen som filen ligger i.</param>
        /// <param name="filNavn">Navnet på filen man vil kopiere.</param>
        static void FilBackup(string filPath, string filNavn)
        {
            FilBackup(filPath, filNavn, filPath);
        }

        /// <summary>
        /// Sjekker om fil er et gyldig filnavn. Returnerer true hvis fil er et gyldig filnavn.
        /// </summary>
        /// <param name="fil">Potensielt filnavn.</param>
        /// <returns>true hvis fil er et gyldig filnavn. false ellers.</returns>
        public static bool SjekkGyldigFilNavn(string fil)
        {
            try
            {
                new System.IO.FileInfo(fil);
                return true;
            }
            catch (System.IO.PathTooLongException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sjekker om mappe er et gyldig mappenavn. Returnerer true hvis mappe er et gyldig mappenavn.
        /// </summary>
        /// <param name="mappe">Potensielt mappenavn.</param>
        /// <returns>true hvis mappe er et gyldig mappenavn. false ellers.</returns>
        public static bool SjekkGyldigMappeNavn(string mappe)
        {
            try
            {
                new System.IO.DirectoryInfo(mappe);
                return true;
            }
            catch (System.IO.PathTooLongException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Hvis filen fil allerede finnes, så legger den "_2" til førstedelen av filnavnet (før . endelsen), og øker tallet til den finner et ledig filnavn.
        /// </summary>
        /// <param name="fil">Fulle paht'en til filen.</param>
        public static void EndreDuplikatFilnavn(ref string fil)
        {
            if (!File.Exists(fil)) return;
            int i;
            for (i = fil.Length - 1; i > 0; i--) if (fil[i] == '\\' || fil[i] == '.') break;
            if (fil[i] != '.') i = fil.Length;
            int teller = 1;
            while (File.Exists(fil)) fil = fil.Insert(i, '_' + (++teller).ToString());
        }
    }
}
