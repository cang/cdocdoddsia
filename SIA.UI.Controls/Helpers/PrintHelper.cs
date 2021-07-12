using System;

namespace SIA.UI.Controls.Helpers
{
	/// <summary>
	/// Summary description for ScanningAndPrinting.
	/// </summary>
	public sealed class PrintHelper
	{
		public static bool IsComponentAvailable
		{
			get {return IsWia2Available();}
		}

		public static bool CanPrint
		{
			get {return IsWia2Available();}
		}

		private static bool IsWia2Available()
		{
			try
			{
                return false;
                //WIA.DeviceManagerClass dmc = new WIA.DeviceManagerClass();
                //return true;
			}
#if DEBUG
            catch (System.Exception exp)
            {
                Console.WriteLine(string.Format("Message: {0}\nStack Trace: {1}", exp.Message, exp.StackTrace));

                return false;
            }
#else
			catch
			{
				return false;
			}
#endif
		}

		public static void Print(System.Windows.Forms.Control owner, string filename)
		{
            //if (!PrintHelper.CanPrint)
            //    throw new InvalidOperationException("Printing is not available");
			
            //WIA.VectorClass vector = new WIA.VectorClass();
            //object tempName = (object)filename;
            //vector.Add(ref tempName, 0);
            //object vector_o = (object)vector;
            //WIA.CommonDialogClass cdc = new WIA.CommonDialogClass();

            //System.Windows.Forms.Form ownedForm = owner.FindForm();
            //bool[] ownedFormsEnabled = null;

            //// Disable the entire UI, otherwise it's possible to close PDN while the
            //// print wizard is active! And then it crashes.
            //if (ownedForm != null)
            //{
            //    ownedFormsEnabled = new bool[ownedForm.OwnedForms.Length];

            //    for (int i = 0; i < ownedForm.OwnedForms.Length; ++i)
            //    {
            //        ownedFormsEnabled[i] = ownedForm.OwnedForms[i].Enabled;
            //        ownedForm.OwnedForms[i].Enabled = false;
            //    }

            //    owner.FindForm().Enabled = false;
            //}

            //cdc.ShowPhotoPrintingWizard(ref vector_o);
			
            //if (ownedForm != null)
            //{
            //    for (int i = 0; i < ownedForm.OwnedForms.Length; ++i)
            //    {
            //        ownedForm.OwnedForms[i].Enabled = ownedFormsEnabled[i];
            //    }

            //    owner.FindForm().Enabled = true;
            //}
		}
	}
}
