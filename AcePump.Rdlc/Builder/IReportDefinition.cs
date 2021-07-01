namespace AcePump.Rdlc.Builder
{
    public interface IReportDefinition
    {
        void SetRdlcPath(RdlcBuilder builder);
        void SetSaveAsName(RdlcBuilder builder);
        void LoadDatasets(RdlcBuilder builder);        
        void AddParams(RdlcBuilder builder);
        void SetProperties(RdlcBuilder builder);
    }
}
