using System;

namespace Keyrox.Shared.Criptography.Contracts {
    public interface ICriptoProvider {

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        byte[] Key { get; set; }

        /// <summary>
        /// Encodes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        byte[] Encode(byte[] buffer);

        /// <summary>
        /// Encodes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        byte[] Encode(byte[] buffer, byte[] key);

        /// <summary>
        /// Decodes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        byte[] Decode(byte[] buffer);

        /// <summary>
        /// Decodes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        byte[] Decode(byte[] buffer, byte[] key);
    }
}
