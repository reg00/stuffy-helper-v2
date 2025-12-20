namespace StuffyHelper.Common.Client;

/// <summary>
/// Record for using files
/// </summary>
/// <param name="Content">File content</param>
/// <param name="FileName">File name</param>
public record FileParam(byte[] Content, string FileName);