To run tests:
1. Open Visual Studio
2. Open solution 'Derivco_API_tests_Doroshchuk.sln'
3. Click 'Test' menu item and choose 'Windows' submenu item
4. Choose 'Test Explorer' item
5. 'Test Explorer' label is appeared into left-side panel
6. Click 'Test Explorer' label to open pane
7. Pin 'Test Explorer' pane
8. Choose 'Derivco_API_tests_Doroshchuk.Tests' tree item and right click on it to run all tests or choose 'TestCompanyAPI'/'TestsEmployeeAPI' to run tests for recources separately

To run tests from command line:
1. Open the cmd prompt as an Administrator.
2. Navigate to the location of the \bin\Debug folder using the CD command.
3. Call the NUnit 2.6.4 Test Runner .exe. Default: “C:\Program Files (x86)\NUnit 2.6.4\bin\nunit-console.exe.
4. Provide name of .dll as argument for Nunit Test Runner. ...
5. Execute command.

There are data-driven tests in solution so it is possible to add relative data from the same equivalence class to run steps of such test with different data.