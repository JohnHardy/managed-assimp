
while True:
    ## Get the structure name.
    sName       = raw_input("Structure Name:")
    sComment    = raw_input("Comment:")

    ## Collect variables.
    lVariables = []
    sValue = ""
    while sValue != "n":
        sVarName = raw_input("\tVariable Name:")
        sVarType = raw_input("\tVariable Type:")
        sVarComment = raw_input("\tVariable Comment:")
        lVariables.append((sVarName, sVarType, sVarComment))
        sValue = raw_input("Another?")


    ## Print out the structure
    print "        #region " + sName + " Structure"
    print "        /** <summary>"+sComment+"</summary> */"
    print "        [StructLayout(LayoutKind.Sequential)]"
    print "        public struct " + sName
    print "        {"
    for sVName, sVType, sVComment in lVariables:
        if sVComment is None: sVComment = "No comment.  See header file."
        print "        \t/** <summary>"+sVComment+"</summary> */"
        print "        \tpublic "+sVType+" " + sVName + ";"
        print "        \t"
    print "        }"
    print "        #endregion"
