using System;
using System.Runtime.InteropServices;
using Artech.Architecture.Common.Packages;
using Artech.Architecture.Common.Services;
using Artech.Architecture.UI.Framework.Packages;
using Artech.Architecture.UI.Framework.Controls;
using Artech.Packages.Definition;
using Artech.Architecture.Common.Events;
using Artech.Architecture.Common.Objects;
using System.Text;
using System.IO;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace F7S.Packages.KBAutoExportObject
{
    [Guid("1B14AAF0-1B74-46CF-BF83-4EC0FAE65A53")]
    public class KBAutoExportObjectPackage : AbstractPackageUI
    {
        public static Guid guid = typeof(KBAutoExportObjectPackage).GUID;

        public override string Name
        {
            get { return "KBAutoExportObject"; }
        }

        public override void Initialize(IGxServiceProvider services)
        {
            base.Initialize(services);
        }

        //private KBAutoExportObject KBAutoExportObject;

        [EventSubscription(ArchitectureEvents.BeforeSaveKBObject)]
        public void OnBeforeSaveKBObject(object sender, KBObjectEventArgs args)
        {
            KBObject obj = args.KBObject;
            StringBuilder buffer = new StringBuilder();
            using (TextWriter writer = new StringWriter(buffer))
                obj.Serialize(writer);
            string objExport = buffer.ToString();

            IOutputService output = CommonServices.Output;
            output.Clear();
            output.StartSection("Iniciando geração do XML");
            string path = obj.GetSignificantName() + ".xml";
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(objExport);
            }
        
            if (File.Exists(path))
            {
                output.StartSection("XML do objeto " + path + " gerado com Sucesso.");
            }
            else
            {
                output.StartSection("Não foi possivel gerar xml do objeto " + path);
            }


        }
    }
}
