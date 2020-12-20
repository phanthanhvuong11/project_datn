using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DATN.Models;
using Microsoft.EntityFrameworkCore;

public class AccountApplication
{
    public bool Register(AccountViewModel model)
    {
        using (var dbContext = new DATNContext()) {
            var dbUser = dbContext.Accounts.FirstOrDefaultAsync(_ => _.UserName == model.UserName).Result;
            if (dbUser != null) return false;

            var account = new Account {
                UserName = model.UserName,
                Password = model.Password
            };
            dbContext.Accounts.Add(account);
            dbContext.SaveChanges();
        }

        var sipPath = "/etc/asterisk/sip.conf";
        var extensionsPath = "/etc/asterisk/extensions.conf";

        using (var sw = File.AppendText(sipPath))
        {
            sw.WriteLine();
            sw.WriteLine($"[{model.UserName}](office-phone)");
            sw.WriteLine($"secret={model.Password}");
            sw.WriteLine("");
            //[2222](office-phone)
            //secret=2222
        }
        using (var sw = File.AppendText(extensionsPath))
        {
            sw.WriteLine();
            sw.WriteLine($"exten => {model.UserName},1,Answer()");
            sw.WriteLine($"same => 2, Dial(SIP/{model.UserName})");
            sw.WriteLine("same => 3, Hangup()");
            sw.WriteLine("");
        }

        RestartAsterisk();
        RestartAsterisk();
        RestartAsterisk();

        return true;
    }

    internal async Task<bool> UpdateAccount(AccountViewModel model)
    {
        await DeleteAccount(model.Id);
        Register(model);
        return true;
    }

    public async Task<bool> DeleteAccount(int id) {
        var sipPath = "/etc/asterisk/sip.conf";
        var extensionsPath = "/etc/asterisk/extensions.conf";

        using (var dbContext = new DATNContext()) {
            var user = await dbContext.Accounts.FirstOrDefaultAsync(_ => _.Id == id);
            if (user == null) return false;
            var sipContent = File.ReadAllText(sipPath);
            sipContent = sipContent.Replace(user.UserName, "");
            File.WriteAllText(sipPath, sipContent);
            //
            var extensionContent = File.ReadAllText(extensionsPath);
            extensionContent = extensionContent.Replace(user.UserName, "");
            File.WriteAllText(extensionsPath, extensionContent);

            dbContext.Remove(user);
            await dbContext.SaveChangesAsync();
            RestartAsterisk();
            RestartAsterisk();
            RestartAsterisk();
            return true;
        }
    }

    public void RestartAsterisk () {
        var process = new System.Diagnostics.Process();
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        process.StartInfo.FileName = "/usr/bin/sudo";
        process.StartInfo.Arguments = "sudo systemctl restart asterisk";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardInput = true;
        process.Start();
        process.Dispose();

    }

    public string GetTerminalContent()
    {
        var process = new System.Diagnostics.Process();
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        process.StartInfo.FileName = "/usr/bin/sudo";
        process.StartInfo.Arguments = "sudo asterisk -r";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardInput = true;
        process.Start();

        // var sw = process.StandardInput;
        process.StandardInput.WriteLine("sip show peers");
        // process.StandardInput.WriteLine("exit");
        var output = new List<string>();

        while (process.StandardOutput.Peek() > 0)
        {
            output.Add(process.StandardOutput.ReadLine());
        }

        process.Dispose();

        var result = "";
        foreach(var line in output) {
            result += line;
            result += "\n";
        }

        return result;
    }

    public async Task<List<AccountViewModel>> GetAccounts() {
        using (var dbContext = new DATNContext()) {
            return await dbContext.Accounts.Select(_ => new AccountViewModel {
                Id = _.Id,
                UserName = _.UserName,
                Password = _.Password
            }).ToListAsync();
        }
    }
}