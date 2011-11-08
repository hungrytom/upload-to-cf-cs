This is a _very_ simple C# binary to upload a file to Rackspace Cloud Files.

Build:

Windows:
    install nant http://nant.sourceforge.net/ and run nant.exe in the directory

Linux (mono):
    
    apt-get install nant (or other method of your distro but that will work for sure in debian/ubuntu).
    nant on the source directory

Run:

Syntax is simple as :

UploadToCFCLI.exe $RCLOUD_API_USER $RCLOUD_API_KEY ${CONTAINER_TO_UPLOAD} ${FILE_TO_UPLOAD}

This is straightfoward enough to don't need a long README.


------------------------------------------------------------------------------------------------

ADDITION BY HUNGRYTOM: 

Thanks very much to chmouel for adding this source. I've added to this in the hope that it will help someone else get started with VS2008 project for Rackspace Cloud Files faster..

If you are building this in Visual Studio, you need to change the Debug command line arguments to include your Rackspace credentials. 

Right-click project->Properties->Debug->Command line arguments->Replace with your details in the text box.