using System.Collections.Generic;
using Abp;
using LockthreatCompliance.Chat.Dto;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
