using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src.Interfaces
{
    public interface IProfileService
    {
        public ICollection<Profile> ProfilesCollection { get; }
        public Profile CurrentProfile { get; set; }
        public bool EditCurrProfile(Profile profile);
        public bool AddNewProfile(Profile profile);
        public bool DeleteProfile(Profile profile);
        public Profile CopyToNewProfile(Profile profile);

    }
}
