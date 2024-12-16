using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baustellen.App.Client.Services.Settings;

internal interface ISettingsService
{
    bool UseMocks { get; set; }
}
