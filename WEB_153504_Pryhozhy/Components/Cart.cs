using Microsoft.AspNetCore.Mvc;

namespace WEB_153504_Pryhozhy.Components
{
    [ViewComponent]
    public class Cart
    {

        public async Task<string> InvokeAsync()
        {
            return await Task.Run(() => "0,00$");
        }
    }
}
