<Query Kind="VBStatements">
  <Connection>
    <ID>5d403b16-7317-4421-824f-3eca0b602c38</ID>
    <Persist>true</Persist>
    <Driver>EntityFrameworkDbContext</Driver>
    <CustomAssemblyPath>C:\Development\mkosbie\AcePump New Development\AcePump.EfDataImpl\bin\Release\AcePump.EfDataImpl.dll</CustomAssemblyPath>
    <CustomTypeName>AcePump.EfDataImpl.AcePumpContext</CustomTypeName>
    <CustomCxString>release</CustomCxString>
    <AppConfigPath>C:\Development\mkosbie\AcePump New Development\AcePump.Web\Web.config</AppConfigPath>
    <DisplayName>Ace Pump Release</DisplayName>
    <IsProduction>true</IsProduction>
  </Connection>
  <Reference Relative="..\..\AcePump New Development\AcePump.EfDataImpl\bin\Release\AcePump.Domain.dll">C:\Development\mkosbie\AcePump New Development\AcePump.EfDataImpl\bin\Release\AcePump.Domain.dll</Reference>
</Query>

dim mismatches as new list(of object)

for each t in pumptemplates
	dim namer as new acepump.domain.models.PumpTemplateNameBuilder(t)
	dim genedName as string = namer.GenerateName()
	
	if t.ConciseSpecificationSummary isnot nothing andalso t.concisespecificationsummary.Trim()<>genedName then 
		mismatches.add(new with {.cur=t.ConciseSpecificationSummary,.gen=genedName,.id=t.PumpTemplateID})
	end if
next

mismatches.Dump()