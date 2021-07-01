namespace AcePump.Rdlc.Builder
{
    public static class RdlcBuilderExtensions
    {
        public static RdlcBuilder LoadReportDefinition(this RdlcBuilder builder, IReportDefinition definition)
        {
            definition.SetRdlcPath(builder);
            definition.SetSaveAsName(builder);
            definition.SetProperties(builder);
            definition.AddParams(builder);
            definition.LoadDatasets(builder);

            return builder;
        }
    }
}
