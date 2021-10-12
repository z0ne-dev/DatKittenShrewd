// <copyright file="ApplicationSettings.cs" company="z0ne">
// Copyright (c) z0ne. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace ThatKittenShrewd
{
    /// <summary>
    ///     Bot options.
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        ///     Gets the discord account token.
        /// </summary>
        [Required]
        public string DiscordToken { get; init; } = null!;
    }
}
