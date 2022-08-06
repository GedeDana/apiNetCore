using Mahas.Components;

namespace DelegasiAPI.Models
{
    [DbTable("SampleTable")]
    public class SampleModel
    {
        [DbKey(true)]
        [DbColumn]
        public int Id { get; set; }

        [DbColumn]
        public string Name { get; set; }
        
    }
}
