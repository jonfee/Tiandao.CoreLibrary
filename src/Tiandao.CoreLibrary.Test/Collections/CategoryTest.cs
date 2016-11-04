﻿using System;
using System.Collections.Generic;

using Xunit;

namespace Tiandao.Collections.Test
{
	public class CategoryTest
	{
		#region 私有字段

		private Category _root;

		#endregion

		#region 构造方法

		public CategoryTest()
		{
			_root = new Category();

			var file = _root.Children.Add("File", "File", "文件");
			var edit = _root.Children.Add("Edit", "Edit", "编辑");
			var help = _root.Children.Add("Help", "Help", "帮助");

			file.Children.Add("Open", "Open", "打开");
			file.Children.Add("Close", "Close", "关闭");
			file.Children.Add("Save", "Save", "保存");
			file.Children.Add("SaveAs", "SaveAs", "另存为");
			file.Children.Add("Recents", "Recents", "最近的文件").Children.AddRange(
				new Category[]
				{
					new Category("Document-1"),
					new Category("Document-2"),
				}
			);
		}

		#endregion

		#region 测试方法

		[Fact]
		public void FindTest()
		{
			Assert.NotNull(_root.Find("File"));
			Assert.NotNull(_root.Find("Edit"));
			Assert.NotNull(_root.Find("Help"));

			Assert.NotNull(_root.Find("File/Save"));
			Assert.NotNull(_root.Find("File/Recents"));
			Assert.NotNull(_root.Find("File/Recents/Document-1"));

			Assert.NotNull(_root.Find("/File/Save"));
			Assert.NotNull(_root.Find("/File/Recents"));
			Assert.NotNull(_root.Find("/File/Recents/Document-2"));

			Assert.NotNull(_root.Find("File").Find("Open"));
			Assert.NotNull(_root.Find("File").Find("./Recents"));
			Assert.NotNull(_root.Find("File").Find("Recents").Find("Document-2"));

			var node = _root.Find("Edit").Find("../File/Save");

			Assert.NotNull(node);
			Assert.Equal("Save", node.Name);
		}

		#endregion
	}
}