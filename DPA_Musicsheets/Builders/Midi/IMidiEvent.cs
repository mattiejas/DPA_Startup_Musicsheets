﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders.Midi
{
    public interface IMidiEvent
    {
        void Handle();
    }
}
