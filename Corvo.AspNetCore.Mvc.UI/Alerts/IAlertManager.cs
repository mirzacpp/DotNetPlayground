using System.Collections.Generic;

namespace Corvo.AspNetCore.Mvc.UI.Alerts
{
    /// <summary>
    /// Defines abstractions for alert manipulations
    /// </summary>
    public interface IAlertManager
    {
        #region Add overloads

        void Add(AlertType alert, string text);

        void Add(AlertType alert, string text, string title);

        void Add(AlertType alert, string text, string title, bool isDismissable);

        void Add(AlertItem alertItem);

        #endregion Add overloads

        #region Add success overloads

        void Success(string text);

        void Success(string text, string title);

        void Success(string text, string title, bool isDismissable);

        #endregion Add success overloads

        #region Add info overloads

        void Info(string text);

        void Info(string text, string title);

        void Info(string text, string title, bool isDismissable);

        #endregion Add info overloads

        IEnumerable<AlertItem> GetAll();

        IEnumerable<AlertItem> GetByType(AlertType alertType);
    }
}