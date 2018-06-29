using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word= Microsoft.Office.Interop.Word;


namespace HANDAZ.PEB.TestConsole
{
    class ExportDataToWord
    {
        public void createWordDocument()
        { 
            //open new document t
            var wordApp = new word.Application();
            wordApp.Visible = true;
            wordApp.Documents.Add();
            wordApp.DisplayAlerts = word.WdAlertLevel.wdAlertsNone;


            //add text to fotters in a document
            foreach (word.Section wordSection in wordApp.ActiveDocument.Sections)
            {
                word.Range footerRange = wordSection.Footers[word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                footerRange.Font.ColorIndex = word.WdColorIndex.wdDarkRed;
                footerRange.Font.Size = 20;
                footerRange.Text = "We are the geeks";
            }

            //add text to headers in a document
            foreach (word.Section section in wordApp.ActiveDocument.Sections)
                {
                    word.Range headerRange = section.Headers[word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphRight;
                }

           
            // selecting a range to write
            word.Range rng = wordApp.ActiveDocument.Range(0, 0);
            rng.Text = " Handaz Steel Calculation Sheet";
            rng.Select();

        }
          



          
        }
    }

