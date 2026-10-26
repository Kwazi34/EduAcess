using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EduAcess.Models;

namespace EduAcess
{
	public static class SeedData
	{
		public static void SeedAll(ApplicationDbContext context)
		{
			SeedInstitutions(context);
			SeedBursaries(context);
			SeedProgrammes(context);
		}

		private static void SeedInstitutions(ApplicationDbContext context)
		{
			if (context.Institutions.Any()) return;

			var institutions = new List<Institution>
			{
                // Universities (26)
                new Institution { Name = "University of Cape Town", ShortName = "UCT", Type = "University", Province = "Western Cape" },
				new Institution { Name = "University of the Witwatersrand", ShortName = "Wits", Type = "University", Province = "Gauteng" },
				new Institution { Name = "Stellenbosch University", ShortName = "SU", Type = "University", Province = "Western Cape" },
				new Institution { Name = "University of Pretoria", ShortName = "UP", Type = "University", Province = "Gauteng" },
				new Institution { Name = "University of KwaZulu-Natal", ShortName = "UKZN", Type = "University", Province = "KwaZulu-Natal" },
				new Institution { Name = "University of Johannesburg", ShortName = "UJ", Type = "University", Province = "Gauteng" },
				new Institution { Name = "University of the Western Cape", ShortName = "UWC", Type = "University", Province = "Western Cape" },
				new Institution { Name = "Rhodes University", ShortName = "RU", Type = "University", Province = "Eastern Cape" },
				new Institution { Name = "University of the Free State", ShortName = "UFS", Type = "University", Province = "Free State" },
				new Institution { Name = "North-West University", ShortName = "NWU", Type = "University", Province = "North West" },
				new Institution { Name = "Nelson Mandela University", ShortName = "NMU", Type = "University", Province = "Eastern Cape" },
				new Institution { Name = "University of South Africa", ShortName = "UNISA", Type = "University", Province = "Gauteng" },
				new Institution { Name = "University of Limpopo", ShortName = "UL", Type = "University", Province = "Limpopo" },
				new Institution { Name = "University of Venda", ShortName = "UNIVEN", Type = "University", Province = "Limpopo" },
				new Institution { Name = "University of Fort Hare", ShortName = "UFH", Type = "University", Province = "Eastern Cape" },
				new Institution { Name = "University of Zululand", ShortName = "UNIZULU", Type = "University", Province = "KwaZulu-Natal" },
				new Institution { Name = "Central University of Technology", ShortName = "CUT", Type = "University", Province = "Free State" },
				new Institution { Name = "Durban University of Technology", ShortName = "DUT", Type = "University", Province = "KwaZulu-Natal" },
				new Institution { Name = "Tshwane University of Technology", ShortName = "TUT", Type = "University", Province = "Gauteng" },
				new Institution { Name = "Cape Peninsula University of Technology", ShortName = "CPUT", Type = "University", Province = "Western Cape" },
				new Institution { Name = "Vaal University of Technology", ShortName = "VUT", Type = "University", Province = "Gauteng" },
				new Institution { Name = "Mangosuthu University of Technology", ShortName = "MUT", Type = "University", Province = "KwaZulu-Natal" },
				new Institution { Name = "Walter Sisulu University", ShortName = "WSU", Type = "University", Province = "Eastern Cape" },
				new Institution { Name = "Sefako Makgatho Health Sciences University", ShortName = "SMU", Type = "University", Province = "Gauteng" },
				new Institution { Name = "Sol Plaatje University", ShortName = "SPU", Type = "University", Province = "Northern Cape" },
				new Institution { Name = "University of Mpumalanga", ShortName = "UMP", Type = "University", Province = "Mpumalanga" },

                // TVET Colleges (50)
                new Institution { Name = "Buffalo City TVET College", ShortName = "BC", Type = "TVET", Province = "Eastern Cape" },
				new Institution { Name = "East Cape Midlands TVET College", ShortName = "ECM", Type = "TVET", Province = "Eastern Cape" },
				new Institution { Name = "Ikhala TVET College", ShortName = "Ikhala", Type = "TVET", Province = "Eastern Cape" },
				new Institution { Name = "Ingwe TVET College", ShortName = "Ingwe", Type = "TVET", Province = "Eastern Cape" },
				new Institution { Name = "King Hintsa TVET College", ShortName = "KH", Type = "TVET", Province = "Eastern Cape" },
				new Institution { Name = "King Sabata Dalindyebo TVET College", ShortName = "KSD", Type = "TVET", Province = "Eastern Cape" },
				new Institution { Name = "Lovedale TVET College", ShortName = "Lovedale", Type = "TVET", Province = "Eastern Cape" },
				new Institution { Name = "Port Elizabeth TVET College", ShortName = "PE", Type = "TVET", Province = "Eastern Cape" },
				new Institution { Name = "Flavius Mareka TVET College", ShortName = "FM", Type = "TVET", Province = "Free State" },
				new Institution { Name = "Goldfields TVET College", ShortName = "Goldfields", Type = "TVET", Province = "Free State" },
				new Institution { Name = "Maluti TVET College", ShortName = "Maluti", Type = "TVET", Province = "Free State" },
				new Institution { Name = "Motheo TVET College", ShortName = "Motheo", Type = "TVET", Province = "Free State" },
				new Institution { Name = "Ekurhuleni East TVET College", ShortName = "EEC", Type = "TVET", Province = "Gauteng" },
				new Institution { Name = "Ekurhuleni West TVET College", ShortName = "EWC", Type = "TVET", Province = "Gauteng" },
				new Institution { Name = "Sedibeng TVET College", ShortName = "Sedibeng", Type = "TVET", Province = "Gauteng" },
				new Institution { Name = "South West Gauteng TVET College", ShortName = "SWGC", Type = "TVET", Province = "Gauteng" },
				new Institution { Name = "Tshwane North TVET College", ShortName = "TNC", Type = "TVET", Province = "Gauteng" },
				new Institution { Name = "Tshwane South TVET College", ShortName = "TSC", Type = "TVET", Province = "Gauteng" },
				new Institution { Name = "Western TVET College", ShortName = "WTVET", Type = "TVET", Province = "Gauteng" },
				new Institution { Name = "Coastal TVET College", ShortName = "Coastal", Type = "TVET", Province = "KwaZulu-Natal" },
				new Institution { Name = "Elangeni TVET College", ShortName = "Elangeni", Type = "TVET", Province = "KwaZulu-Natal" },
				new Institution { Name = "Esayidi TVET College", ShortName = "Esayidi", Type = "TVET", Province = "KwaZulu-Natal" },
				new Institution { Name = "Majuba TVET College", ShortName = "Majuba", Type = "TVET", Province = "KwaZulu-Natal" },
				new Institution { Name = "Mnambithi TVET College", ShortName = "Mnambithi", Type = "TVET", Province = "KwaZulu-Natal" },
				new Institution { Name = "Mthashana TVET College", ShortName = "Mthashana", Type = "TVET", Province = "KwaZulu-Natal" },
				new Institution { Name = "Thekwini TVET College", ShortName = "Thekwini", Type = "TVET", Province = "KwaZulu-Natal" },
				new Institution { Name = "Umfolozi TVET College", ShortName = "Umfolozi", Type = "TVET", Province = "KwaZulu-Natal" },
				new Institution { Name = "Umgungundlovu TVET College", ShortName = "Umgungundlovu", Type = "TVET", Province = "KwaZulu-Natal" },
				new Institution { Name = "Capricorn TVET College", ShortName = "Capricorn", Type = "TVET", Province = "Limpopo" },
				new Institution { Name = "Lephalale TVET College", ShortName = "Lephalale", Type = "TVET", Province = "Limpopo" },
				new Institution { Name = "Letaba TVET College", ShortName = "Letaba", Type = "TVET", Province = "Limpopo" },
				new Institution { Name = "Mopani South East TVET College", ShortName = "Mopani", Type = "TVET", Province = "Limpopo" },
				new Institution { Name = "Sekhukhune TVET College", ShortName = "Sekhukhune", Type = "TVET", Province = "Limpopo" },
				new Institution { Name = "Vhembe TVET College", ShortName = "Vhembe", Type = "TVET", Province = "Limpopo" },
				new Institution { Name = "Waterberg TVET College", ShortName = "Waterberg", Type = "TVET", Province = "Limpopo" },
				new Institution { Name = "Ehlanzeni TVET College", ShortName = "Ehlanzeni", Type = "TVET", Province = "Mpumalanga" },
				new Institution { Name = "Gert Sibande TVET College", ShortName = "Gert Sibande", Type = "TVET", Province = "Mpumalanga" },
				new Institution { Name = "Nkangala TVET College", ShortName = "Nkangala", Type = "TVET", Province = "Mpumalanga" },
				new Institution { Name = "Orbit TVET College", ShortName = "Orbit", Type = "TVET", Province = "North West" },
				new Institution { Name = "Taletso TVET College", ShortName = "Taletso", Type = "TVET", Province = "North West" },
				new Institution { Name = "Vuselela TVET College", ShortName = "Vuselela", Type = "TVET", Province = "North West" },
				new Institution { Name = "Northern Cape Rural TVET College", ShortName = "NCR", Type = "TVET", Province = "Northern Cape" },
				new Institution { Name = "Northern Cape Urban TVET College", ShortName = "NCU", Type = "TVET", Province = "Northern Cape" },
				new Institution { Name = "Boland TVET College", ShortName = "Boland", Type = "TVET", Province = "Western Cape" },
				new Institution { Name = "College of Cape Town", ShortName = "CCT", Type = "TVET", Province = "Western Cape" },
				new Institution { Name = "False Bay TVET College", ShortName = "False Bay", Type = "TVET", Province = "Western Cape" },
				new Institution { Name = "Northlink TVET College", ShortName = "Northlink", Type = "TVET", Province = "Western Cape" },
				new Institution { Name = "South Cape TVET College", ShortName = "South Cape", Type = "TVET", Province = "Western Cape" },
				new Institution { Name = "West Coast TVET College", ShortName = "West Coast", Type = "TVET", Province = "Western Cape" },

                // Private (a representative sample — 15 majors)
                new Institution { Name = "Boston City Campus", ShortName = "Boston", Type = "Private", Province = "Gauteng" },
				new Institution { Name = "The Independent Institute of Education (IIE)", ShortName = "IIE", Type = "Private", Province = "Gauteng" },
				new Institution { Name = "Inscape Education Group", ShortName = "Inscape", Type = "Private", Province = "Gauteng" },
				new Institution { Name = "Rosebank College", ShortName = "Rosebank", Type = "Private", Province = "Gauteng" },
				new Institution { Name = "Damelin", ShortName = "Damelin", Type = "Private", Province = "Gauteng" },
				new Institution { Name = "Varsity College", ShortName = "VC", Type = "Private", Province = "KwaZulu-Natal" },
				new Institution { Name = "MANCOSA", ShortName = "MANCOSA", Type = "Private", Province = "KwaZulu-Natal" },
				new Institution { Name = "Regent Business School", ShortName = "Regent", Type = "Private", Province = "KwaZulu-Natal" },
				new Institution { Name = "Stadio", ShortName = "Stadio", Type = "Private", Province = "Western Cape" },
				new Institution { Name = "Red and Yellow Creative School of Business", ShortName = "Red & Yellow", Type = "Private", Province = "Western Cape" },
				new Institution { Name = "AFDA", ShortName = "AFDA", Type = "Private", Province = "Gauteng" },
				new Institution { Name = "Milpark Education", ShortName = "Milpark", Type = "Private", Province = "Gauteng" },
				new Institution { Name = "Eduvos", ShortName = "Eduvos", Type = "Private", Province = "Gauteng" },
				new Institution { Name = "Richfield Graduate Institute of Technology", ShortName = "Richfield", Type = "Private", Province = "KwaZulu-Natal" },
				new Institution { Name = "SACAP", ShortName = "SACAP", Type = "Private", Province = "Western Cape" }
			};

			context.Institutions.AddRange(institutions);
			context.SaveChanges();
		}

		private static void SeedBursaries(ApplicationDbContext context)
		{
			if (context.Bursaries.Any()) return;

			var bursaries = new List<Bursary>
			{
				new Bursary { Name = "NSFAS Bursary", Provider = "National Student Financial Aid Scheme", FieldOfStudy = "All Fields (Public Universities & TVET)", OpeningDate = new DateTime(2025, 9, 15), ClosingDate = new DateTime(2026, 10, 31), Description = "Full cost of study for students with household income under R350,000/year.", Website = "https://www.nsfas.org.za", IsActive = true },
				new Bursary { Name = "Funza Lushaka Bursary", Provider = "Department of Basic Education", FieldOfStudy = "Teaching (BEd)", OpeningDate = new DateTime(2025, 10, 1), ClosingDate = new DateTime(2026, 1, 24), Description = "Full cost bursary for future teachers.", Website = "https://www.funzalushaka.doe.gov.za", IsActive = true },
				new Bursary { Name = "Sasol Bursary", Provider = "Sasol", FieldOfStudy = "Engineering, Science, IT, Commerce", OpeningDate = new DateTime(2026, 2, 1), ClosingDate = new DateTime(2026, 5, 17), Description = "Full tuition and living allowance.", Website = "https://www.sasolbursaries.com", IsActive = true },
				new Bursary { Name = "Sappi Engineering Bursary", Provider = "Sappi", FieldOfStudy = "Engineering", OpeningDate = new DateTime(2026, 7, 1), ClosingDate = new DateTime(2026, 9, 30), Description = "For SA citizens aged 17-25.", Website = "https://www.sappi.com", IsActive = true },
				new Bursary { Name = "MTN Bursary", Provider = "MTN Foundation", FieldOfStudy = "Engineering, IT", OpeningDate = new DateTime(2026, 3, 1), ClosingDate = new DateTime(2026, 5, 31), Description = "Full tuition, laptop, and data.", Website = "https://www.mtn.co.za", IsActive = true },
				new Bursary { Name = "Standard Bank Group Bursary", Provider = "Standard Bank", FieldOfStudy = "Finance, Banking, IT", OpeningDate = new DateTime(2026, 6, 1), ClosingDate = new DateTime(2026, 8, 31), Description = "Full funding with mentorship.", Website = "https://www.standardbank.co.za", IsActive = true },
				new Bursary { Name = "Investec Bursary", Provider = "Investec", FieldOfStudy = "Commerce, Finance, IT", OpeningDate = new DateTime(2026, 4, 1), ClosingDate = new DateTime(2026, 9, 30), Description = "Full tuition and mentorship.", Website = "https://www.investec.com", IsActive = true },
				new Bursary { Name = "SAICA Thuthuka Bursary", Provider = "SAICA", FieldOfStudy = "BCom Accounting (CA Stream)", OpeningDate = new DateTime(2026, 3, 1), ClosingDate = new DateTime(2026, 8, 31), Description = "For Black African and Coloured CA(SA) students.", Website = "https://www.saica.co.za", IsActive = true },
				new Bursary { Name = "BANKSETA Bursary", Provider = "BANKSETA", FieldOfStudy = "Banking, Finance, IT", OpeningDate = new DateTime(2026, 8, 1), ClosingDate = new DateTime(2026, 11, 13), Description = "Up to R120,000 per learner per year.", Website = "https://www.bankseta.org.za", IsActive = true },
				new Bursary { Name = "Mandela Rhodes Scholarship", Provider = "Mandela Rhodes Foundation", FieldOfStudy = "Postgraduate", OpeningDate = new DateTime(2026, 1, 15), ClosingDate = new DateTime(2026, 4, 14), Description = "For African citizens aged 19-29.", Website = "https://www.mandelarhodes.org", IsActive = true },
				new Bursary { Name = "Allan Gray Orbis Fellowship", Provider = "Allan Gray Orbis Foundation", FieldOfStudy = "Commerce, Science, Engineering, Law", OpeningDate = new DateTime(2026, 1, 10), ClosingDate = new DateTime(2026, 3, 31), Description = "Full cost plus entrepreneurship programme.", Website = "https://www.allangrayorbis.org", IsActive = true }
			};

			context.Bursaries.AddRange(bursaries);
			context.SaveChanges();
		}
		private static void SeedProgrammes(ApplicationDbContext context)
		{
			if (context.Programmes.Any()) return;

			// Freshly reload institutions from DB (not cached)
			var inst = context.Institutions.AsNoTracking().ToList();

			if (!inst.Any())
			{
				return; // Nothing to link programmes to
			}

			int InstId(string shortName)
			{
				var found = inst.FirstOrDefault(i => i.ShortName == shortName);
				return found?.Id ?? 0;
			}

			var programmes = new List<Programme>
	{
        // ============ UCT ============
        new Programme { Name = "Bachelor of Commerce (BCom)", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 30, DurationYears = 3, RequiredSubjects = "Maths 50%+, English 50%+", InstitutionId = InstId("UCT") },
		new Programme { Name = "Bachelor of Business Science (BBusSc)", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 32, DurationYears = 4, RequiredSubjects = "Maths 60%+, English 50%+", InstitutionId = InstId("UCT") },
		new Programme { Name = "BSc Computer Science", QualificationType = "Degree", FieldOfStudy = "IT", RequiredAps = 30, DurationYears = 3, RequiredSubjects = "Maths 60%+, Physical Science 50%+", InstitutionId = InstId("UCT") },
		new Programme { Name = "BSc Engineering (Chemical)", QualificationType = "Degree", FieldOfStudy = "Engineering", RequiredAps = 33, DurationYears = 4, RequiredSubjects = "Maths 70%+, Physical Science 70%+", InstitutionId = InstId("UCT") },
		new Programme { Name = "MBChB (Medicine)", QualificationType = "Degree", FieldOfStudy = "Health", RequiredAps = 38, DurationYears = 6, RequiredSubjects = "Maths 70%+, Physical Science 70%+, Life Sciences 70%+", InstitutionId = InstId("UCT") },
		new Programme { Name = "BA Psychology", QualificationType = "Degree", FieldOfStudy = "Arts", RequiredAps = 24, DurationYears = 3, RequiredSubjects = "English 50%+", InstitutionId = InstId("UCT") },

        // ============ WITS ============
        new Programme { Name = "BCom Accounting", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 28, DurationYears = 3, RequiredSubjects = "Maths 60%+, English 50%+", InstitutionId = InstId("Wits") },
		new Programme { Name = "BSc Computer Science", QualificationType = "Degree", FieldOfStudy = "IT", RequiredAps = 32, DurationYears = 3, RequiredSubjects = "Maths 70%+, Physical Science 60%+", InstitutionId = InstId("Wits") },
		new Programme { Name = "BSc Engineering (Civil)", QualificationType = "Degree", FieldOfStudy = "Engineering", RequiredAps = 34, DurationYears = 4, RequiredSubjects = "Maths 70%+, Physical Science 70%+", InstitutionId = InstId("Wits") },
		new Programme { Name = "MBChB (Medicine)", QualificationType = "Degree", FieldOfStudy = "Health", RequiredAps = 38, DurationYears = 6, RequiredSubjects = "Maths 70%+, Physical Science 70%+", InstitutionId = InstId("Wits") },
		new Programme { Name = "BA Fine Arts", QualificationType = "Degree", FieldOfStudy = "Arts", RequiredAps = 22, DurationYears = 3, RequiredSubjects = "English 50%+, Portfolio required", InstitutionId = InstId("Wits") },

        // ============ UP ============
        new Programme { Name = "BCom (Mainstream)", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 30, DurationYears = 3, RequiredSubjects = "English 60%+, Maths 50%+", InstitutionId = InstId("UP") },
		new Programme { Name = "BCom (4-year programme)", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 26, DurationYears = 4, RequiredSubjects = "English 60%+, Maths 40%+", InstitutionId = InstId("UP") },
		new Programme { Name = "BEng Mechanical Engineering", QualificationType = "Degree", FieldOfStudy = "Engineering", RequiredAps = 32, DurationYears = 4, RequiredSubjects = "Maths 70%+, Physical Science 60%+", InstitutionId = InstId("UP") },
		new Programme { Name = "BSc Computer Science", QualificationType = "Degree", FieldOfStudy = "IT", RequiredAps = 30, DurationYears = 3, RequiredSubjects = "Maths 60%+, Physical Science 50%+", InstitutionId = InstId("UP") },

        // ============ UJ ============
        new Programme { Name = "BCom Accounting", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 26, DurationYears = 3, RequiredSubjects = "Maths 50%+, English 50%+", InstitutionId = InstId("UJ") },
		new Programme { Name = "BEd Foundation Phase Teaching", QualificationType = "Degree", FieldOfStudy = "Teaching", RequiredAps = 28, DurationYears = 4, RequiredSubjects = "English 60%+, Maths 40%+", InstitutionId = InstId("UJ") },
		new Programme { Name = "BSc Information Technology", QualificationType = "Degree", FieldOfStudy = "IT", RequiredAps = 28, DurationYears = 3, RequiredSubjects = "Maths 50%+, English 50%+", InstitutionId = InstId("UJ") },
		new Programme { Name = "Diploma in Architecture", QualificationType = "Diploma", FieldOfStudy = "Engineering", RequiredAps = 24, DurationYears = 3, RequiredSubjects = "Maths 50%+, English 50%+", InstitutionId = InstId("UJ") },

        // ============ UKZN ============
        new Programme { Name = "BCom in Accounting", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 28, DurationYears = 3, RequiredSubjects = "Maths 50%+, English 50%+", InstitutionId = InstId("UKZN") },
		new Programme { Name = "BCom in Accounting (Extended)", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 24, DurationYears = 4, RequiredSubjects = "Maths 40%+, English 50%+", InstitutionId = InstId("UKZN") },
		new Programme { Name = "Bachelor of Laws (LLB)", QualificationType = "Degree", FieldOfStudy = "Law", RequiredAps = 30, DurationYears = 4, RequiredSubjects = "English 60%+", InstitutionId = InstId("UKZN") },
		new Programme { Name = "Bachelor of Architectural Studies", QualificationType = "Degree", FieldOfStudy = "Engineering", RequiredAps = 30, DurationYears = 3, RequiredSubjects = "Maths 60%+ (Not Maths Literacy), Portfolio required", InstitutionId = InstId("UKZN") },

        // ============ SU ============
        new Programme { Name = "BCom (General)", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 30, DurationYears = 3, RequiredSubjects = "Maths 60%+, English 50%+", InstitutionId = InstId("SU") },
		new Programme { Name = "BA in Visual Arts", QualificationType = "Degree", FieldOfStudy = "Arts", RequiredAps = 28, DurationYears = 3, RequiredSubjects = "English 50%+, Portfolio required", InstitutionId = InstId("SU") },

        // ============ CPUT ============
        new Programme { Name = "Diploma in Engineering", QualificationType = "Diploma", FieldOfStudy = "Engineering", RequiredAps = 22, DurationYears = 3, RequiredSubjects = "Maths 50%+, Physical Science 50%+", InstitutionId = InstId("CPUT") },
		new Programme { Name = "Diploma in Business and Management Sciences", QualificationType = "Diploma", FieldOfStudy = "Business", RequiredAps = 20, DurationYears = 3, RequiredSubjects = "English 50%+, Maths 40%+", InstitutionId = InstId("CPUT") },

        // ============ TVET ============
        new Programme { Name = "N4-N6 Financial Management", QualificationType = "N4-N6", FieldOfStudy = "Business", RequiredAps = 15, DurationYears = 2, RequiredSubjects = "Grade 12 pass, Maths Literacy 40%+", InstitutionId = InstId("Thekwini") },
		new Programme { Name = "N4-N6 Business Management", QualificationType = "N4-N6", FieldOfStudy = "Business", RequiredAps = 15, DurationYears = 2, RequiredSubjects = "Grade 12 pass, English 40%+", InstitutionId = InstId("Elangeni") },
		new Programme { Name = "N4-N6 Electrical Engineering", QualificationType = "N4-N6", FieldOfStudy = "Engineering", RequiredAps = 15, DurationYears = 2, RequiredSubjects = "Grade 12 pass, Maths 40%+, Physical Science 40%+", InstitutionId = InstId("EEC") },
		new Programme { Name = "N4-N6 Mechanical Engineering", QualificationType = "N4-N6", FieldOfStudy = "Engineering", RequiredAps = 15, DurationYears = 2, RequiredSubjects = "Grade 12 pass, Maths 40%+, Physical Science 40%+", InstitutionId = InstId("TNC") },
		new Programme { Name = "N4-N6 Hospitality Management", QualificationType = "N4-N6", FieldOfStudy = "Business", RequiredAps = 14, DurationYears = 2, RequiredSubjects = "Grade 12 pass, English 40%+", InstitutionId = InstId("CCT") },

        // ============ Private ============
        new Programme { Name = "Higher Certificate in Business Management", QualificationType = "Higher Certificate", FieldOfStudy = "Business", RequiredAps = 18, DurationYears = 1, RequiredSubjects = "English 40%+", InstitutionId = InstId("Boston") },
		new Programme { Name = "Diploma in Information Technology", QualificationType = "Diploma", FieldOfStudy = "IT", RequiredAps = 20, DurationYears = 2, RequiredSubjects = "Maths 40%+", InstitutionId = InstId("Boston") },
		new Programme { Name = "Bachelor of Business Administration", QualificationType = "Degree", FieldOfStudy = "Business", RequiredAps = 22, DurationYears = 3, RequiredSubjects = "English 50%+", InstitutionId = InstId("IIE") },
		new Programme { Name = "Higher Certificate in Marketing", QualificationType = "Higher Certificate", FieldOfStudy = "Business", RequiredAps = 18, DurationYears = 1, RequiredSubjects = "English 40%+", InstitutionId = InstId("Rosebank") },
		new Programme { Name = "Diploma in Graphic Design", QualificationType = "Diploma", FieldOfStudy = "Arts", RequiredAps = 20, DurationYears = 2, RequiredSubjects = "English 50%+, Portfolio required", InstitutionId = InstId("Inscape") },
		new Programme { Name = "Diploma in Logistics", QualificationType = "Diploma", FieldOfStudy = "Business", RequiredAps = 20, DurationYears = 2, RequiredSubjects = "English 50%+, Maths 40%+", InstitutionId = InstId("VC") }
	};

			// Only add programmes whose institution exists
			var validProgrammes = programmes
				.Where(p => p.InstitutionId > 0 && inst.Any(i => i.Id == p.InstitutionId))
				.ToList();

			if (validProgrammes.Any())
			{
				context.Programmes.AddRange(validProgrammes);
				context.SaveChanges();
			}
		}
	}
}