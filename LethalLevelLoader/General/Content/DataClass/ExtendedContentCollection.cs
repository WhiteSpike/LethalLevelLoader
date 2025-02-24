using System.Collections.Generic;

namespace LethalLevelLoader
{
	/// <summary>
	/// Data class responsible of storing the different extended content types
	/// </summary>
	/// <typeparam name="T">Type of extended content stored in the collection</typeparam>
	public class ExtendedContentCollection<T> where T : ExtendedContent
	{
		bool IsCacheValid = true;
		private List<T> allContent;
		/// <summary>
		/// List of extended content, both vanilla and custom
		/// </summary>
		public List<T> AllContent
		{
			get
			{
				if (allContent == null || !IsCacheValid)
				{
					allContent = [.. VanillaContent, .. CustomContent];
					IsCacheValid = true;
				}

				return allContent;
			}
		}
		/// <summary>
		/// List of extended content which is considered vanilla
		/// </summary>
		public List<T> VanillaContent { get; internal set; } = [];
		/// <summary>
		/// List of extended content which is considered custom
		/// </summary>
		public List<T> CustomContent { get; internal set; } = [];
		/// <summary>
		/// Adds extended content to the collection
		/// </summary>
		/// <param name="content">The content that is added to the collection</param>
		public void Add(T content)
		{
			switch (content.ContentType)
			{
				case ContentType.Vanilla:
					VanillaContent.Add(content);
					break;
				case ContentType.Custom:
					CustomContent.Add(content);
					break;
				case ContentType.Any:
					VanillaContent.Add(content);
					CustomContent.Add(content);
					break;
			}
			IsCacheValid = false;
		}
		/// <summary>
		/// Checks if the specified content is already present in the collection
		/// </summary>
		/// <param name="content">Content that may be inserted or not in the collection</param>
		/// <returns>Wether the specified content is already present in the collection</returns>
		public bool Contains(T content)
		{
			switch(content.ContentType)
			{
				case ContentType.Vanilla:
					return VanillaContent.Contains(content);
				case ContentType.Custom:
					return CustomContent.Contains(content);
				case ContentType.Any:
					return VanillaContent.Contains(content) && CustomContent.Contains(content);
			}
			return false;
		}
	}
}
