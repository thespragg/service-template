using System.Text.Json;

namespace Server.Policies.Json;

internal class ScreamingSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        var stringBuilder = new System.Text.StringBuilder();
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c) && i > 0)
            {
                stringBuilder.Append('_');
            }
            stringBuilder.Append(char.ToUpper(c));
        }
        return stringBuilder.ToString();
    }
}