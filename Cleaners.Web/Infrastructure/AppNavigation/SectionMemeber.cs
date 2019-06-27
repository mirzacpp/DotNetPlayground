namespace Cleaners.Web.Infrastructure.AppNavigation
{
    /// <summary>
    /// Default section member implementation
    /// </summary>
    public class SectionMemeber<TSection> /*: ISectionMember<TSection>*/
    {
        public SectionMemeber(TSection section, bool isAvailable)
        {
            Section = section;
            IsAvailable = isAvailable;
        }

        public TSection Section { get; set; }

        public bool IsAvailable { get; set; }
    }
}