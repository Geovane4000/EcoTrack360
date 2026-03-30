use EcoTrack360;

db.EnvironmentalImpacts.insertMany([
  { CompanyName: "Empresa 1", CarbonFootprint: 100, WaterUsage: 999, WasteGenerated: 20 },
  { CompanyName: "Empresa 2", CarbonFootprint: 200, WaterUsage: 100, WasteGenerated: 40 }
]);

db.SocialActions.insertMany([
  { CompanyName: "Empresa 1", EmployeesInvolved: 5, CommunityProjects: 1 },
  { CompanyName: "Empresa 2", EmployeesInvolved: 10, CommunityProjects: 2 }
]);

db.GovernanceIndicators.insertMany([
  { CompanyName: "Empresa 1", BoardDiversity: 30, ExecutiveCompensation: 100000 },
  { CompanyName: "Empresa 2", BoardDiversity: 40, ExecutiveCompensation: 120000 }
]);

db.SustainableProjects.insertMany([
  { CompanyName: "Empresa 1", NumberOfProjects: 2, InvestmentAmount: 50000 },
  { CompanyName: "Empresa 2", NumberOfProjects: 3, InvestmentAmount: 80000 }
]);

db.Employees.insertMany([
  { Name: "Funcionario 1", CompanyName: "Empresa 1", VolunteerHours: 3 },
  { Name: "Funcionario 2", CompanyName: "Empresa 2", VolunteerHours: 6 }
]);

db.EnvironmentalImpacts.find({ CompanyName: "Empresa 1" });

db.EnvironmentalImpacts.updateOne(
  { CompanyName: "Empresa 1" },
  { $set: { WaterUsage: 888 } }
);

db.SocialActions.deleteOne({ CompanyName: "Empresa 2" });
