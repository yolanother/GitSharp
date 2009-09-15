		    FileHeader fh = Data(string.Empty);
			Assert.AreEqual(-1, fh.parseGitFileName(0, fh.Buffer.Length));
		    Assert.IsNotNull(fh.Hunks);
		    Assert.IsTrue(fh.Hunks.Count == 0);
		    FileHeader fh = Data("a/ b/");
			Assert.AreEqual(-1, fh.parseGitFileName(0, fh.Buffer.Length));
		    FileHeader fh = Data("\n");
			Assert.AreEqual(-1, fh.parseGitFileName(0, fh.Buffer.Length));
		    FileHeader fh = Data("\n\n");
			Assert.AreEqual(1, fh.parseGitFileName(0, fh.Buffer.Length));
		    const string name = "foo";
		    FileHeader fh = Header(name);
			Assert.AreEqual(GitLine(name).Length, fh.parseGitFileName(0, fh.Buffer.Length));
		    Assert.AreEqual(name, fh.OldName);
		    Assert.AreSame(fh.OldName, fh.NewName);
		    FileHeader fh = Data("a/foo b/bar\n-");
			Assert.IsTrue(fh.parseGitFileName(0, fh.Buffer.Length) > 0);
		    Assert.IsNull(fh.OldName);
		    Assert.IsNull(fh.NewName);
		    const string name = "foo bar";
		    FileHeader fh = Header(name);
		    Assert.AreEqual(GitLine(name).Length, fh.parseGitFileName(0,
					fh.Buffer.Length));
		    Assert.AreEqual(name, fh.OldName);
		    Assert.AreSame(fh.OldName, fh.NewName);
		    const string name = "foo\tbar";
		    const string dqName = "foo\\tbar";
		    FileHeader fh = DqHeader(dqName);
		    Assert.AreEqual(DqGitLine(dqName).Length, fh.parseGitFileName(0,
					fh.Buffer.Length));
		    Assert.AreEqual(name, fh.OldName);
		    Assert.AreSame(fh.OldName, fh.NewName);
		    const string name = "foo \n\0bar";
		    const string dqName = "foo \\n\\0bar";
		    FileHeader fh = DqHeader(dqName);
		    Assert.AreEqual(DqGitLine(dqName).Length, fh.parseGitFileName(0,
					fh.Buffer.Length));
		    Assert.AreEqual(name, fh.OldName);
		    Assert.AreSame(fh.OldName, fh.NewName);
		    const string name = "src/foo/bar/argh/code.c";
		    FileHeader fh = Header(name);
		    Assert.AreEqual(GitLine(name).Length, fh.parseGitFileName(0,
					fh.Buffer.Length));
		    Assert.AreEqual(name, fh.OldName);
		    Assert.AreSame(fh.OldName, fh.NewName);
		    const string name = "src/foo/bar/argh/code.c";
		    const string header = "project-v-1.0/" + name + " mydev/" + name + "\n";
		    FileHeader fh = Data(header + "-");
			Assert.AreEqual(header.Length, fh.parseGitFileName(0, fh.Buffer.Length));
		    Assert.AreEqual(name, fh.OldName);
		    Assert.AreSame(fh.OldName, fh.NewName);
		    FileHeader fh = Data("diff --git \"a/\\303\\205ngstr\\303\\266m\" \"b/\\303\\205ngstr\\303\\266m\"\n"
				    + "new file mode 100644\n"
		    AssertParse(fh);
		    Assert.AreEqual("/dev/null", fh.OldName);
		    Assert.AreSame(FileHeader.DEV_NULL, fh.OldName);
		    Assert.AreEqual("\u00c5ngstr\u00f6m", fh.NewName);
            Assert.AreEqual(FileHeader.ChangeTypeEnum.ADD, fh.getChangeType());
            Assert.AreEqual(FileHeader.PatchTypeEnum.UNIFIED, fh.getPatchType());
		    Assert.AreSame(FileMode.Missing, fh.GetOldMode());
		    Assert.AreSame(FileMode.RegularFile, fh.NewMode);
		    FileHeader fh = Data("diff --git \"a/\\303\\205ngstr\\303\\266m\" \"b/\\303\\205ngstr\\303\\266m\"\n"
				    + "deleted file mode 100644\n"
		    AssertParse(fh);
		    Assert.AreEqual("\u00c5ngstr\u00f6m", fh.OldName);
		    Assert.AreEqual("/dev/null", fh.NewName);
		    Assert.AreSame(FileHeader.DEV_NULL, fh.NewName);

            Assert.AreEqual(FileHeader.ChangeTypeEnum.DELETE, fh.getChangeType());
            Assert.AreEqual(FileHeader.PatchTypeEnum.UNIFIED, fh.getPatchType());
		    Assert.AreSame(FileMode.RegularFile, fh.GetOldMode());
		    Assert.AreSame(FileMode.Missing, fh.NewMode);
		    FileHeader fh = Data("diff --git a/a b b/a b\n"
		    AssertParse(fh);
		    Assert.AreEqual("a b", fh.OldName);
		    Assert.AreEqual("a b", fh.NewName);

		    Assert.AreEqual(FileHeader.ChangeTypeEnum.MODIFY, fh.getChangeType());
            Assert.AreEqual(FileHeader.PatchTypeEnum.UNIFIED, fh.getPatchType());
		    Assert.AreSame(FileMode.RegularFile, fh.GetOldMode());
		    Assert.AreSame(FileMode.ExecutableFile, fh.NewMode);
		    FileHeader fh = Data("diff --git a/a b/ c/\\303\\205ngstr\\303\\266m\n"

			int ptr = fh.parseGitFileName(0, fh.Buffer.Length);
		    Assert.IsNull(fh.OldName); // can't parse names on a rename
		    Assert.IsNull(fh.NewName);
			ptr = fh.parseGitHeaders(ptr, fh.Buffer.Length);
		    Assert.AreEqual("a", fh.OldName);
		    Assert.AreEqual(" c/\u00c5ngstr\u00f6m", fh.NewName);
		    Assert.AreEqual(FileHeader.ChangeTypeEnum.RENAME, fh.getChangeType());
            Assert.AreEqual(FileHeader.PatchTypeEnum.UNIFIED, fh.getPatchType());
		    Assert.IsNull(fh.GetOldMode());
		    Assert.IsNull(fh.NewMode);
		    FileHeader fh = Data("diff --git a/a b/ c/\\303\\205ngstr\\303\\266m\n"

			int ptr = fh.parseGitFileName(0, fh.Buffer.Length);
		    Assert.IsNull(fh.OldName); // can't parse names on a rename
		    Assert.IsNull(fh.NewName);
			ptr = fh.parseGitHeaders(ptr, fh.Buffer.Length);
		    Assert.AreEqual("a", fh.OldName);
		    Assert.AreEqual(" c/\u00c5ngstr\u00f6m", fh.NewName);
            Assert.AreEqual(FileHeader.ChangeTypeEnum.RENAME, fh.getChangeType());
            Assert.AreEqual(FileHeader.PatchTypeEnum.UNIFIED, fh.getPatchType());
		    Assert.IsNull(fh.GetOldMode());
		    Assert.IsNull(fh.NewMode);
		    FileHeader fh = Data("diff --git a/a b/ c/\\303\\205ngstr\\303\\266m\n"

			int ptr = fh.parseGitFileName(0, fh.Buffer.Length);
		    Assert.IsNull(fh.OldName); // can't parse names on a copy
		    Assert.IsNull(fh.NewName);
			ptr = fh.parseGitHeaders(ptr, fh.Buffer.Length);
		    Assert.AreEqual("a", fh.OldName);
		    Assert.AreEqual(" c/\u00c5ngstr\u00f6m", fh.NewName);
		    Assert.AreEqual(FileHeader.ChangeTypeEnum.COPY, fh.getChangeType());
		    Assert.AreEqual(FileHeader.PatchTypeEnum.UNIFIED, fh.getPatchType());
		    Assert.IsNull(fh.GetOldMode());
		    Assert.IsNull(fh.NewMode);
		    const string oid = "78981922613b2afb6025042ff6bd878ac1994e85";
		    const string nid = "61780798228d17af2d34fce4cfbdf35556832472";
		    FileHeader fh = Data("diff --git a/a b/a\n" + "index " + oid
		    AssertParse(fh);
		    Assert.AreEqual("a", fh.OldName);
		    Assert.AreEqual("a", fh.NewName);

		    Assert.AreSame(FileMode.RegularFile, fh.GetOldMode());
		    Assert.AreSame(FileMode.RegularFile, fh.NewMode);
		    const string oid = "78981922613b2afb6025042ff6bd878ac1994e85";
		    const string nid = "61780798228d17af2d34fce4cfbdf35556832472";
		    FileHeader fh = Data("diff --git a/a b/a\n" + "index " + oid
		    AssertParse(fh);

		    Assert.AreEqual("a", fh.OldName);
		    Assert.AreEqual("a", fh.NewName);
		    Assert.IsNull(fh.GetOldMode());
		    Assert.IsNull(fh.NewMode);
		    const int a = 7;
		    const string oid = "78981922613b2afb6025042ff6bd878ac1994e85";
		    const string nid = "61780798228d17af2d34fce4cfbdf35556832472";
		    FileHeader fh = Data("diff --git a/a b/a\n" + "index "
		    AssertParse(fh);
		    Assert.AreEqual("a", fh.OldName);
		    Assert.AreEqual("a", fh.NewName);

		    Assert.AreSame(FileMode.RegularFile, fh.GetOldMode());
		    Assert.AreSame(FileMode.RegularFile, fh.NewMode);
		    const int a = 7;
		    const string oid = "78981922613b2afb6025042ff6bd878ac1994e85";
		    const string nid = "61780798228d17af2d34fce4cfbdf35556832472";
		    FileHeader fh = Data("diff --git a/a b/a\n" + "index "
		    AssertParse(fh);

		    Assert.AreEqual("a", fh.OldName);
		    Assert.AreEqual("a", fh.NewName);
		    Assert.IsNull(fh.GetOldMode());
		    Assert.IsNull(fh.NewMode);
	    private static void AssertParse(FileHeader fh)
			int ptr = fh.parseGitFileName(0, fh.Buffer.Length);
			ptr = fh.parseGitHeaders(ptr, fh.Buffer.Length);
	    private static FileHeader Data(string inStr)
	    private static FileHeader Header(string path)
		    return Data(GitLine(path) + "--- " + path + "\n");
	    private static string GitLine(string path)
	    private static FileHeader DqHeader(string path)
		    return Data(DqGitLine(path) + "--- " + path + "\n");
	    private static string DqGitLine(string path)