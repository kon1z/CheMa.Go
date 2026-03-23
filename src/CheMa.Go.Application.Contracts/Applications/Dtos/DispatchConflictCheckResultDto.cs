using System.Collections.Generic;

namespace CheMa.Go.Applications.Dtos;

public class DispatchConflictCheckResultDto
{
    public bool HasConflict => Conflicts.Count > 0;

    public List<DispatchConflictItemDto> Conflicts { get; set; } = new();
}
