using System;
using System.Collections.Generic;
using System.Text;
using DevTools.Models;

namespace DevTools.Interfaces
{
    public interface ISettingsService
    {
        AppSettings Settings { get; }
        void Save();
    }
}
