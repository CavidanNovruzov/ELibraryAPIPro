using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Dtos;

public record UploadFileDto(Stream Content, string FileName);
