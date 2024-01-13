using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CharacterCreationNamespace
{
    public interface IDatabaseOperations
    {
        void GetCharacterCount();
        void SaveCharacter();

        void DisplayCharacterList();
        void LoadAllCharacter();

        void LoadSpecificCharacter();

        void DeleteAllCharacter();
        void DeleteSpecificCharacter();
    }

    public abstract class GameFeatures
    {
        public abstract void DisplayCharacterDetails(Character character);
    }

    // class for character appearance features
    public class CharacterAppearance
    {
        public string BodyType { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string SkinColor { get; set; }
        public string UpperClothing { get; set; }
        public string LowerClothing { get; set; }
        public string Accessories { get; set; }
    }

    // class for character weapons features
    public class CharacterWeapons
    {
        public string PrimaryWeapon { get; set; }
        public string SecondaryWeapon { get; set; }
    }

    public class CharacterStats
    {
        public int Charisma { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Luck { get; set; }
        public int Dexterity { get; set; }
    }
    public struct CharacterFeatures
    {
        // properties/features of the character 
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public bool IsRoyal { get; set; }
        public CharacterAppearance Appearance { get; set; }
        public CharacterStats Stats { get; set; }
        public string Companion { get; set; }
        public CharacterWeapons Weapons { get; set; }
    }

    public class Character : GameFeatures, IDatabaseOperations
    {
        private const string CONNECTION_STRING = "Data Source=(localdb)\\MSSQLLocalDB;AttachDbFilename=C:\\USERS\\JOHNU\\SOURCE\\REPOS\\CHARACTERCREATION\\CHARACTERCREATION\\CHARACTERCREATIONDATABASE.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;MultiSubnetFailover=False";
        public CharacterFeatures Features { get; set; }

        private int charactersInDatabase;
        public Character()
        {

        }
        public Character(string name, string gender, string race, string characterClass, bool royal,
            CharacterAppearance appearance, CharacterStats stats, string companion, CharacterWeapons weapons)
        {
            this.Features = new CharacterFeatures
            {
                Name = name,
                Gender = gender,
                Race = race,
                Class = characterClass,
                IsRoyal = royal,
                Appearance = appearance,
                Stats = stats,
                Companion = companion,
                Weapons = weapons
            };
        }

        public string GetConnectionString()
        {
            return CONNECTION_STRING;
        }

        public void GetCharacterCount()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    connection.Open();

                    string countQuery = "SELECT COUNT(*) FROM CharacterDetailsTable";

                    using (SqlCommand command = new SqlCommand(countQuery, connection))
                    {
                        charactersInDatabase = (int)command.ExecuteScalar();
                    }
                }
            } catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new CharacterDatabaseException("Error encountered during the retrieval of character's quantity");
            }
        }

        public override void DisplayCharacterDetails(Character character)
        {
            Console.WriteLine("{0, -50}{1, -50}", "\n\t\tDETAILS", "APPEARANCE");
            Console.WriteLine("{0, -50}{1, -50}", $"Name: {character.Features.Name}", $"Body Type: {character.Features.Appearance.BodyType}");
            Console.WriteLine("{0, -50}{1, -50}", $"Gender: {character.Features.Gender}", $"Skin Color: {character.Features.Appearance.SkinColor}");
            Console.WriteLine("{0, -50}{1, -50}", $"Race: {character.Features.Race}", $"Eye Color: {character.Features.Appearance.EyeColor}");
            Console.WriteLine("{0, -50}{1, -50}", $"Class: {character.Features.Class}", $"Hair Color: {character.Features.Appearance.HairColor}");
            Console.WriteLine("{0, -50}{1, -50}", $"Royal: {character.Features.IsRoyal}", $"Upper Clothing: {character.Features.Appearance.UpperClothing}");
            Console.WriteLine("{0, -50}{1, -50}", $"Companion: {character.Features.Companion}", $"Lower Clothing: {character.Features.Appearance.LowerClothing}");
            Console.WriteLine("{0, -50}{1, -50}", "", $"Accessories: {character.Features.Appearance.Accessories}");
            Console.WriteLine("{0, -50}{1, -50}", "\n\t\tSTATS", "\tWEAPONS");
            Console.WriteLine("{0, -50}{1, -50}", $"Charisma: {character.Features.Stats.Charisma}", $"Primary Weapon: {character.Features.Weapons.PrimaryWeapon}");
            Console.WriteLine("{0, -50}{1, -50}", $"Agility: {character.Features.Stats.Agility}", $"Secondary Weapon: {character.Features.Weapons.SecondaryWeapon}");
            Console.WriteLine("{0, -50}{1, -50}", $"Strength: {character.Features.Stats.Strength}", "");
            Console.WriteLine("{0, -50}{1, -50}", $"Intelligence: {character.Features.Stats.Intelligence}", "");
            Console.WriteLine("{0, -50}{1, -50}", $"Luck: {character.Features.Stats.Luck}", "");
            Console.WriteLine("{0, -50}{1, -50}", $"Dexterity: {character.Features.Stats.Dexterity}", "");
        }

        public void SaveCharacter()
        {
            SaveCharacter(CONNECTION_STRING);
        }
        public void SaveCharacter(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO CharacterDetailsTable " +
                                        "(name, gender, race, class, charisma, agility, strength, intelligence, luck, " +
                                        "dexterity, royal, body_type, skin_color, eye_color, hair_color, upper_clothing, " +
                                        "lower_clothing, accessories, companion, primary_weapon, secondary_weapon) " +
                                        "VALUES " +
                                        "(@Name, @Gender, @Race, @Class, @Charisma, @Agility, @Strength, @Intelligence, @Luck, " +
                                        "@Dexterity, @Royal, @BodyType, @SkinColor, @EyeColor, @HairColor, @UpperClothing, " +
                                        "@LowerClothing, @Accessories, @Companion, @PrimaryWeapon, @SecondaryWeapon)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", Features.Name);
                        command.Parameters.AddWithValue("@Gender", Features.Gender);
                        command.Parameters.AddWithValue("@Race", Features.Race);
                        command.Parameters.AddWithValue("@Class", Features.Class);
                        command.Parameters.AddWithValue("@Charisma", Features.Stats.Charisma);
                        command.Parameters.AddWithValue("@Agility", Features.Stats.Agility);
                        command.Parameters.AddWithValue("@Strength", Features.Stats.Strength);
                        command.Parameters.AddWithValue("@Intelligence", Features.Stats.Intelligence);
                        command.Parameters.AddWithValue("@Luck", Features.Stats.Luck);
                        command.Parameters.AddWithValue("@Dexterity", Features.Stats.Dexterity);
                        command.Parameters.AddWithValue("@Royal", Features.IsRoyal);
                        command.Parameters.AddWithValue("@BodyType", Features.Appearance.BodyType);
                        command.Parameters.AddWithValue("@SkinColor", Features.Appearance.SkinColor);
                        command.Parameters.AddWithValue("@EyeColor", Features.Appearance.EyeColor);
                        command.Parameters.AddWithValue("@HairColor", Features.Appearance.HairColor);
                        command.Parameters.AddWithValue("@UpperClothing", Features.Appearance.UpperClothing);
                        command.Parameters.AddWithValue("@LowerClothing", Features.Appearance.LowerClothing);
                        command.Parameters.AddWithValue("@Accessories", Features.Appearance.Accessories);
                        command.Parameters.AddWithValue("@Companion", Features.Companion);
                        command.Parameters.AddWithValue("@PrimaryWeapon", Features.Weapons.PrimaryWeapon);
                        command.Parameters.AddWithValue("@SecondaryWeapon", Features.Weapons.SecondaryWeapon);

                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("\nTaking you back to main menu...\n");
                MainProgram.Main(new string[0]);
            } catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new CharacterDatabaseException("Error are encountered during character saving operation.");
            }
            
        }

        public void DisplayCharacterList()
        {
            DisplayCharacterList(CONNECTION_STRING);
        }

        public void DisplayCharacterList(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT name FROM CharacterDetailsTable";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int index = 1;

                            while (reader.Read())
                            {
                                string charName = reader["name"].ToString();
                                Console.WriteLine($"{(index)}. {charName}");
                                index++;
                            }
                        }
                    }
                }
            } catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new CharacterDatabaseException("Error are encountered during the selection of the characters to display.");
            }
        }
        public void LoadAllCharacter()
        {
            LoadAllCharacter(CONNECTION_STRING);
        }
        public void LoadAllCharacter(string connectionString)
        {
            if(charactersInDatabase == 0)
            {
                Console.WriteLine("\nThe characters' list is currently empty.\n");
                Console.WriteLine("Taking you back to main menu..\n");
                MainProgram.Main(new string[0]);
            }

            Console.WriteLine("\n\t\t\t---List of Characters---");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM CharacterDetailsTable";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                displayLoadedCharacter(reader);
                                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                            }
                        }
                    }
                }
                Console.WriteLine("\nTaking you back to main menu...\n");
                MainProgram.Main(new string[0]);
            } catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new CharacterDatabaseException("Error encountered during the selection of the characters to load.");
            }
        }
        public void LoadSpecificCharacter()
        {
            LoadSpecificCharacter(CONNECTION_STRING);
        }
        public void LoadSpecificCharacter(string connectionString)
        {
            if (charactersInDatabase == 0)
            {
                Console.WriteLine("\nThe characters' list is currently empty.\n");
                Console.WriteLine("Taking you back to main menu..\n");

                MainProgram.Main(new string[0]);
            }

            Console.WriteLine("\n---List of Characters---");
            DisplayCharacterList();

            int selection;

            do
            {
                Console.Write("Enter the number of the character you want to load: ");
                if(int.TryParse(Console.ReadLine(), out selection))
                {
                    if(selection < 1 || selection > charactersInDatabase)
                    {
                        Console.WriteLine("\nInvalid input. Please enter a valid number.");
                    } else
                    {
                        break;
                    }
                } else
                {
                    Console.WriteLine("\nInvalid input. Please enter a number.");
                }

            } while (true);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM CharacterDetailsTable WHERE name = @Name";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", GetCharacterName(selection));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                displayLoadedCharacter(reader);
                            }
                            else
                            {
                                Console.WriteLine("Character details not found.");
                            }
                        }
                    }
                }
            } catch (SqlException e) 
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new CharacterDatabaseException("Error encountered during the selection of the character to load.");
            }
            Console.WriteLine("\nTaking you back to main menu...\n");
            MainProgram.Main(new string[0]);
        }

        public string GetCharacterName(int index)
        {
            return GetCharacterName(index, CONNECTION_STRING);
        }

        public string GetCharacterName(int index, string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM CharacterDetailsTable";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int currentIndex = 1;

                            while (reader.Read())
                            {
                                if (currentIndex == index)
                                {
                                    return reader["name"].ToString();
                                }

                                currentIndex++;
                            }
                        }
                    }
                }
                return "";
            } catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new CharacterDatabaseException("Error encountered during the selection of the character's name");
            }
            
        }

        public void DeleteAllCharacter()
        {
            DeleteAllCharacter(CONNECTION_STRING);
        }
        public void DeleteAllCharacter(string connectionString)
        {
            if (charactersInDatabase == 0)
            {
                Console.WriteLine("\nThe characters' list is currently empty.\n");
                Console.WriteLine("Taking you back to main menu..\n");
                MainProgram.Main(new string[0]);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteAllQuery = "DELETE FROM CharacterDetailsTable";

                    using (SqlCommand command = new SqlCommand(deleteAllQuery, connection))
                    {
                        Console.WriteLine("\nDeleting characters...");
                        command.ExecuteNonQuery();
                        Console.WriteLine("All characters are deleted successfully.");
                    }
                }
            } catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new CharacterDatabaseException("Error encountered during the deletion of the characters.");
            }
        }

        public void DeleteSpecificCharacter()
        {
            DeleteSpecificCharacter(CONNECTION_STRING);
        }
        public void DeleteSpecificCharacter(string connectionString)
        {
            if (charactersInDatabase == 0)
            {
                Console.WriteLine("\nThe characters' list is currently empty.\n");

                Console.WriteLine("Taking you back to main menu..\n");

                MainProgram.Main(new string[0]);
            }

            Console.WriteLine("List of Characters");
            DisplayCharacterList();

            int selection;
            do
            {
                Console.Write("Enter the number of the character you want to delete: ");
                if (int.TryParse(Console.ReadLine(), out selection))
                {
                    if (selection < 1 || selection > charactersInDatabase)
                    {
                        Console.WriteLine("\nInvalid input. Please enter a valid number.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter a number.");
                }

            } while (true);

            string name = GetCharacterName(selection);


            int choice;
            Console.WriteLine($"\nAre you sure you want to delete {name}?");
            Console.WriteLine("(1) Yes (2) No");
            do
            {
                Console.Write("Enter your input here: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice > 2 || choice < 1)
                    {
                        Console.WriteLine("\nInvalid input. Please enter a valid number.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter a number.");
                }
            } while (true);

            if(choice == 1)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                    {
                        connection.Open();

                        string deleteQuery = "DELETE FROM CharacterDetailsTable WHERE name = @Name";

                        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Name", name);
                            command.ExecuteNonQuery();
                            Console.WriteLine($"{name} has been successfully deleted.");
                        }
                    }

                    Console.WriteLine("\nTaking you back to main menu...\n");

                    MainProgram.Main(new string[0]);
                } catch(Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    throw new CharacterDatabaseException("Error encountered during the deletion of specific character.");
                }
            } else if (choice == 2)
            {

                Console.WriteLine("\nTaking you back to main menu...\n");

                MainProgram.Main(new string[0]);
            }
        }

        public void displayLoadedCharacter(SqlDataReader reader)
        {
            string name = reader["name"].ToString();
            string gender = reader["gender"].ToString();
            string race = reader["race"].ToString();
            string charClass = reader["class"].ToString();
            bool royal = (bool)reader["royal"];
            string companion = reader["companion"].ToString();

            CharacterStats stats = new CharacterStats
            {
                Charisma = (int)reader["charisma"],
                Agility = (int)reader["agility"],
                Strength = (int)reader["strength"],
                Intelligence = (int)reader["intelligence"],
                Luck = (int)reader["luck"],
                Dexterity = (int)reader["dexterity"]
            };

            CharacterAppearance appearance = new CharacterAppearance
            {
                BodyType = reader["body_type"].ToString(),
                HairColor = reader["hair_color"].ToString(),
                EyeColor = reader["eye_color"].ToString(),
                SkinColor = reader["skin_color"].ToString(),
                UpperClothing = reader["upper_clothing"].ToString(),
                LowerClothing = reader["lower_clothing"].ToString(),
                Accessories = reader["accessories"].ToString()
            };

            CharacterWeapons weapons = new CharacterWeapons
            {
                PrimaryWeapon = reader["primary_weapon"].ToString(),
                SecondaryWeapon = reader["secondary_weapon"].ToString()
            };

            Features = new CharacterFeatures
            {
                Name = name,
                Gender = gender,
                Race = race,
                Class = charClass,
                IsRoyal = royal,
                Appearance = appearance,
                Stats = stats,
                Companion = companion,
                Weapons = weapons
            };
            Character loadedCharacter = new Character(Features.Name, Features.Gender, 
                                                      Features.Race, Features.Class, Features.IsRoyal,
                                                      Features.Appearance, Features.Stats, 
                                                      Features.Companion, Features.Weapons);

            DisplayCharacterDetails(loadedCharacter);
        }
    }

    public class MainProgram
    {
        public static void Main(string[] args)
        {
            try
            {
                Character character = new Character();
                character.GetCharacterCount();
                Console.WriteLine("---Character Creation Program---");
                Console.WriteLine("(1) Create new character \n" +
                    "(2) Load character \n" +
                    "(3) Delete character\n" +
                    "(4) Exit");

                int selection = ValidateSelection(4);

                switch (selection)
                {
                    case 1:
                        CreateCharacter(character);
                        break;
                    case 2:
                        LoadCharacter(character);
                        break;
                    case 3:
                        DeleteCharacter(character);
                        break;
                    case 4:

                        Console.WriteLine("\nExiting the program...");
                        Console.WriteLine("\nProgram closed successfully. Thank you!");
                        Environment.Exit(0);
                        break;
                }
            } catch (CharacterCreationSystemException e)
            {
                Console.WriteLine(e.Message);
            } catch (CharacterDatabaseException e)
            {
                Console.WriteLine(e.Message);
            }catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        public static void DeleteCharacter(Character character)
        {
            Console.WriteLine("\n(1) Delete All Characters (2) Delete Specific Character");

            int selection = ValidateSelection(2);

            if (selection == 1)
            {
                Console.WriteLine("\nAre you sure you want to delete all of the characters?");
                Console.WriteLine("(1) Yes (2) No");

                int choice = ValidateSelection(2);

                if(choice == 1)
                {
                    character.DeleteAllCharacter();
                } else if (choice == 2)
                {
                    Main(new string[0]);
                }
            } else if (selection == 2)
            {
                character.DeleteSpecificCharacter();
            }
        }
        public static void LoadCharacter(Character character)
        {
            Console.WriteLine("\n(1) Load All Characters (2) Load Specific Character");

            int selection = ValidateSelection(2);

            if (selection == 1)
            {
                character.LoadAllCharacter();
            } else if (selection == 2)             {
                character.LoadSpecificCharacter();
            }
        }

        public static void CreateCharacter(Character character)
        {

            int charisma = 5;
            int agility = 5;
            int strength = 5;
            int intelligence = 5;
            int luck = 5;
            int dexterity = 5;

            Console.WriteLine("\n'Welcome to the immersive world of our MMORPG! \nDive into a realm of endless possibilities, where your character's destiny is in your hands. \nEmbark on epic quests, conquer challenges, and shape your unique path in this online adventure!'");

            Console.WriteLine("\n--Name--");
            string name = NameSelection(character);

            Console.WriteLine("\n--Gender--\n(1) Male (2) Female");
            string gender = GenderSelection();

            Console.WriteLine("\n--Choose your race--");
            string race = RaceSelection();

            string characterClass;

            Console.WriteLine("\n--Choose your class--");

            if (race == "Demon")
            {
                characterClass = DemonClassSelection();
            }
            else if (race == "Angel")
            {
                characterClass = AngelClassSelection();
            }
            else
            {
                characterClass = NormalClassSelection();
            }

            ListDefaultStats();
            charisma = StatsValue("Charisma");
            agility = StatsValue("Agility");
            strength = StatsValue("Strength");
            intelligence = StatsValue("Intelligence");
            luck = StatsValue("Luck");
            dexterity = StatsValue("Dexterity");

            Console.WriteLine("\nDo you want your character to have a royal bloodline?: ");
            bool isRoyal = IsRoyalBlood();

            Console.WriteLine("\n--Body Type--");
            string bodyType = BodyTypeSelection();

            Console.WriteLine("\n--Skin Color--");
            string skinColor = SkinColorSelection();

            Console.WriteLine("\n--Eye Color--");
            string eyeColor = ColorSelection();

            Console.WriteLine("\n--Hair Color--");
            string hairColor = ColorSelection();

            Console.WriteLine("\n--Upper Clothing--");
            string upperClothing = UpperClothingSelection();

            Console.WriteLine("\n--Lower Clothing--");
            string lowerClothing = LowerClothingSelection();

            Console.WriteLine("\n--Accessories--");
            string accessories = AccessoriesSelection();

            Console.WriteLine("\n--Companion--");
            string companion = CompanionSelection();

            Console.WriteLine("\n--Primary Weapon--");
            string primaryWeapon = PrimaryWeaponSelection(characterClass);

            Console.WriteLine("\n--Secondary Weapon--");
            string secondaryWeapon = SecondaryWeaponSelection();

            CharacterAppearance appearance = new CharacterAppearance
            {
                BodyType = bodyType,
                EyeColor = eyeColor,
                HairColor = hairColor,
                SkinColor = skinColor,
                UpperClothing = upperClothing,
                LowerClothing = lowerClothing,
                Accessories = accessories
            };

            CharacterStats stats = new CharacterStats
            {
                Charisma = charisma,
                Agility = agility,
                Strength = strength,
                Intelligence = intelligence,
                Luck = luck,
                Dexterity = dexterity
            };

            CharacterWeapons weapons = new CharacterWeapons
            {
                PrimaryWeapon = primaryWeapon,
                SecondaryWeapon = secondaryWeapon
            };

            CharacterFeatures features = new CharacterFeatures
            {
                Name = name,
                Gender = gender,
                Race = race,
                Class = characterClass,
                IsRoyal = isRoyal,
                Appearance = appearance,
                Stats = stats,
                Companion = companion,
                Weapons = weapons
            };

            character = new Character(features.Name, features.Gender, features.Race, features.Class, features.IsRoyal, features.Appearance,
                                                features.Stats, features.Companion, features.Weapons);

            Console.WriteLine($"{name} has been created successfully!");
            Console.WriteLine();
            character.DisplayCharacterDetails(character);
            character.SaveCharacter();
        }
        
        public static string NameSelection(Character character)
        {
            string pattern = @"^.{4,16}$";
            string name;
            do
            {
                Console.Write("Enter you input here: ");
                name = Console.ReadLine().Trim();

                if (name == "")
                {
                    Console.WriteLine("\nName shouldn't be empty.");
                    continue;
                }

                if(!Regex.IsMatch(name, pattern))
                {
                    Console.WriteLine("\nThe name should have a minimum of 4 characters and maximum of 16 characters.");
                    continue;
                }

                if(!ValidateName(name, character))
                {
                    Console.WriteLine($"\n{name} already exists.");
                    throw new CharacterCreationSystemException("An error encountered cause the user entered an already existing name.");
                }

                break;
            } while(true);

            return name;
        }

        public static bool ValidateName(string name, Character character)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(character.GetConnectionString()))
                {
                    connection.Open();

                    string selectQuery = "SELECT COUNT(*) FROM CharacterDetailsTable WHERE name = @Name";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);

                        int count = (int)command.ExecuteScalar();

                        if(count > 0)
                        {
                            return false;
                        }
                    } 
                }
                return true;
            } catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new CharacterDatabaseException("An error occured during the validation of the character's name within the database.");
            }
        }

        public static int ValidateSelection(int max)
        {
            int selection;
            do
            {
                Console.Write("Enter your input here: ");
                if (int.TryParse(Console.ReadLine(), out selection))
                {
                    if (selection > max || selection < 1)
                    {
                        Console.WriteLine("\nInvalid input. Please enter a valid number.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter a number.");
                }
            } while (true);

            return selection;
        }

        public static void ListDefaultStats()
        {
            Console.WriteLine("\nNote: The limit points for each stats is 20");
            Console.WriteLine("These are the default value for the stats:\n" +
                "Charisma: 5\n" +
                "Agility: 5\n" +
                "Strength: 5\n" +
                "Intelligence: 5\n" +
                "Luck: 5\n" +
                "Dexterity: 5\n");
        }



        // ------------------------FEATURES SELECTION METHODS BELOW-----------------------
        public static string SecondaryWeaponSelection()
        {
            Console.WriteLine("(1) Enchanted Dagger\n" +
                    "(2) Off-hand Focus Orb\n" +
                    "(3) Battle Tomahawk\n" +
                    "(4) Mystic Cane\n" +
                    "(5) Elemental Shortsword");

            int selection = ValidateSelection(5);

            switch (selection)
            {
                case 1: return "Enchanted Dagger";
                case 2: return "Off-hand Focus Orb";
                case 3: return "Battle Tomahawk";
                case 4: return "Mystic Cane";
                case 5: return "Elemental Shortsword";
            }
            return "";
        }

        public static string PrimaryWeaponSelection(string charClass)
        {
            string primary = "";

            switch (charClass)
            {
                case "Mage":

                    Console.WriteLine("(1) Staff of Arcane Power\n" +
                        "(2) Elemental Grimoire\n" +
                        "(3) Crystal Wand\n" +
                        "(4) Scepter of the Celestial Magi\n" +
                        "(5) Runed Dagger");

                    break;
                case "Fighter":
                    Console.WriteLine("(1) Greatsword\n" +
                        "(2) Warhammer\n" +
                        "(3) Dual Axes\n" +
                        "(4) Longsword\n" +
                        "(5) Polearm");
                    break;
                case "Assassin":
                    Console.WriteLine("(1) Twin Daggers\n" +
                        "(2) Poisoned Shortsword\n" +
                        "(3) Concealed Blade Gauntlets\n" +
                        "(4) Curved Kukri\n" +
                        "(5) Shrouded Katana");
                    break;
                case "Marksman":
                    Console.WriteLine("(1) Longbow\n" +
                        "(2) Crossbow\n" +
                        "(3) Scoped Rifle\n" +
                        "(4) Repeating Hand Crossbow\n" +
                        "(5) Composite Bow");
                    break;
                case "Healer":
                    Console.WriteLine("(1) Healing Staff\n" +
                        "(2) Divine Tome\n" +
                        "(3) Life-infused Wand\n" +
                        "(4) Sacred Ankh\n" +
                        "(5) Potion Belt");
                    break;
                case "Necromancer":
                    Console.WriteLine("(1) Bone Staff\n" +
                        "(2) Soul Grimoire\n" +
                        "(3) Death's Scythe\n" +
                        "(4) Dark Orb Scepter\n" +
                        "(5) Necrotic Dagger");
                    break;
                case "Witch":
                    Console.WriteLine("(1) Cauldron of Hexes\n" +
                        "(2) Witch's Broomstick\n" +
                        "(3) Enchanted Crystal Ball\n" +
                        "(4) Spellbound Grimoire\n" +
                        "(5) Arcane Wand");
                    break;
                case "Paladin":
                    Console.WriteLine("(1) Holy Avenger Sword\n" +
                        "(2) Witch's Broomstick\n" +
                        "(3) Radiant Lance\n" +
                        "(4) Aegis of Light (Large Shield)\n" +
                        "(5) Heavenly Warhammer");
                    break;
                case "Oracle":
                    Console.WriteLine("(1) Mystic Crystal Ball\n" +
                        "(2) Runed Divination Cards\n" +
                        "(3) Oracle's Scepter\n" +
                        "(4) Celestial Harp\n" +
                        "(5) Ancestral Talisman");
                    break;
            }

            int selection = ValidateSelection(5);

            switch (charClass)
            {
                // mage
                case "Mage":

                    switch (selection)
                    {
                        case 1: return "Staff of Arcane Power";
                        case 2: return "Elemental Grimoire";
                        case 3: return "Crystal Wand";
                        case 4: return "Scepter of the Celestial Magi";
                        case 5: return "Runed Dagger";
                    }
                    break;

                // fighter
                case "Fighter":
                    switch (selection)
                    {
                        case 1: return "Greatsword";
                        case 2: return "Warhammer";
                        case 3: return "Dual Axes";
                        case 4: return "Longsword";
                        case 5: return "Polearm";
                    }
                    break;
                // assassin 
                case "Assassin":
                    switch (selection)
                    {
                        case 1: return "Twin Daggers";
                        case 2: return "Poisoned Shortsword";
                        case 3: return "Concealed Blade Gauntlets";
                        case 4: return "Curved Kukri";
                        case 5: return "Shrouded Katana";
                    }
                    break;
                // marksman
                case "Marksman":
                    switch (selection)
                    {
                        case 1: return "Longbow";
                        case 2: return "Crossbow";
                        case 3: return "Scoped Rifle";
                        case 4: return "Repeating Hand Crossbow";
                        case 5: return "Composite Bow";
                    }
                    break;
                // healer
                case "Healer":
                    switch (selection)
                    {
                        case 1: return "Healing Staff";
                        case 2: return "Divine Tome";
                        case 3: return "Life-infused Wand";
                        case 4: return "Sacred Ankh";
                        case 5: return "Potion Belt";
                    }
                    break;
                // necromancer
                case "Necromancer":
                    switch (selection)
                    {
                        case 1: return "Bone Staff";
                        case 2: return "Soul Grimoire";
                        case 3: return "Death's Scythe";
                        case 4: return "Dark Orb Scepter";
                        case 5: return "Necrotic Dagger";
                    }
                    break;
                // witch
                case "Witch":
                    switch (selection)
                    {
                        case 1: return "Cauldron of Hexes";
                        case 2: return "Witch's Broomstick";
                        case 3: return "Enchanted Crystal Ball";
                        case 4: return "Spellbound Grimoire";
                        case 5: return "Arcane Wand";
                    }
                    break;
                // paladin
                case "Paladin":
                    switch (selection)
                    {
                        case 1: return "Holy Avenger Sword";
                        case 2: return "Divine Mace and Shield";
                        case 3: return "Radiant Lance";
                        case 4: return "Aegis of Light (Large Shield)";
                        case 5: return "Heavenly Warhammer";
                    }
                    break;
                // oracle
                case "Oracle":
                    switch (selection)
                    {
                        case 1: return "Mystic Crystal Ball";
                        case 2: return "Runed Divination Cards";
                        case 3: return "Oracle's Scepter";
                        case 4: return "Celestial Harp";
                        case 5: return "Ancestral Talisman";
                    }
                    break;
            }

            return primary;
        }

        public static string CompanionSelection()
        {
            string companion = "";

            Console.WriteLine("(1) Dragon\n" +
                    "(2) Saber tooth\n" +
                    "(3) Unicorn\n" +
                    "(4) Serpent\n" +
                    "(5) Phoenix\n" +
                    "(6) Chimera\n" +
                    "(7) Dog\n" +
                    "(8) Cat\n" +
                    "(9) Hamster\n" +
                    "(10) Mammoth");

            int selection = ValidateSelection(10);

            switch (selection)
            {
                case 1: return "Dragon";
                case 2: return "Saber tooth";
                case 3: return "Unicorn";
                case 4: return "Serpent";
                case 5: return "Phoenix";
                case 6: return "Chimera";
                case 7: return "Dog";
                case 8: return "Cat";
                case 9: return "Hamster";
                case 10: return "Mammoth";
            }

            return companion;
        }
        public static string AccessoriesSelection()
        {
            string accessories = "";

            Console.WriteLine("(1) Ornate Pendant\n" +
                    "(2) Wide-brimmed Hat\n" +
                    "(3) Embroidered Belt\n" +
                    "(4) Leather Gloves\n" +
                    "(5) Traveler's Satchel");

            int selection = ValidateSelection(5);

            switch (selection)
            {
                case 1: return "Ornate Pendant";
                case 2: return "Wide-brimmed Hat";
                case 3: return "Embroidered Belt";
                case 4: return "Leather Gloves";
                case 5: return "Traveler's Satchel";
            }

            return accessories;
        }
        public static string LowerClothingSelection()
        {
            string clothes = "";

            Console.WriteLine("(1) Utility Pants\n" +
                    "(2) Nomad's Attire\n" +
                    "(3) Drifter's Trousers\n" +
                    "(4) Traveler's Leggings\n" +
                    "(5) Padded Breeches");

            int selection = ValidateSelection(5);

            switch (selection)
            {
                case 1: return "Utility Pants";
                case 2: return "Nomad's Attire";
                case 3: return "Drifter's Trousers";
                case 4: return "Traveler's Leggings";
                case 5: return "Padded Breeches";
            }

            return clothes;
        }
        public static string UpperClothingSelection()
        {
            string clothes = "";

            Console.WriteLine("(1) Chainmail Chestplate\n" +
                    "(2) Adventurer's Tunic\n" +
                    "(3) Traveler's Cloak\n" +
                    "(4) Standard Robe\n" +
                    "(5) Casual Shirt");

            int selection = ValidateSelection(5);

            switch (selection)
            {
                case 1: return "Chainmail Chestplate";
                case 2: return "Adventurer's Tunic";
                case 3: return "Traveler's Cloak";
                case 4: return "Standard Robe";
                case 5: return "Casual Shirt";
            }

            return clothes;
        }

        public static string ColorSelection()
        {
            
            string color = "";

            Console.WriteLine("(1) Black\n" +
                    "(2) Brown\n" +
                    "(3) Green\n" +
                    "(4) Blue\n" +
                    "(5) Red\n" +
                    "(6) Orange\n" +
                    "(7) Yellow\n" +
                    "(8) Violet\n" +
                    "(9) Pink\n" +
                    "(10) Maroon\n" +
                    "(11) Gray\n" +
                    "(12) White");

            int selection = ValidateSelection(12);

            switch (selection)
            {
                case 1: return "Black";
                case 2: return "Brown";
                case 3: return "Green";
                case 4: return "Blue";
                case 5: return "Red";
                case 6: return "Orange";
                case 7: return "Yellow";
                case 8: return "Violet";
                case 9: return "Pink";
                case 10: return "Maroon";
                case 11: return "Gray";
                case 12: return "White";
            }

            return color;

        }
        public static string SkinColorSelection()
        {
            string skinColor = "";

            Console.WriteLine("(1) Pale\n" +
                    "(2) Fair\n" +
                    "(3) Tan\n" +
                    "(4) Black");

            int selection = ValidateSelection(4);

            switch (selection)
            {
                case 1: return "Pale";
                case 2: return "Fair";
                case 3: return "Tan";
                case 4: return "Dark";
            }

            return skinColor;
        }

        public static string BodyTypeSelection()
        {
            string bodyType = "";

            Console.WriteLine("(1) Slim\n" +
                    "(2) Normal\n" +
                    "(3) Fat\n" +
                    "(4) Obese");

            int selection = ValidateSelection(4);

            switch (selection)
            {
                case 1: return "Slim";
                case 2: return "Normal";
                case 3: return "Fat";
                case 4: return "Obese";
            }

            return bodyType;
        }

        public static int StatsValue(string stat)
        {
            int value = 0;
            do
            {
                Console.Write($"{stat}: ");
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    if(value > 20)
                    {
                        Console.WriteLine("Invalid input. The maximum points for each stats during creation is 20.");
                    } else if (value < 5)
                    {
                        Console.WriteLine("Invalid input. The minimum points for each stats is 5.");
                    } else
                    {
                        break;
                    }
                } else
                {
                    Console.WriteLine("Invalid input. Please input a number.");
                }
            } while (true);

            return value;
        }

        public static string GenderSelection()
        {
            string gender;

            int selection = ValidateSelection(2);

            if (selection == 1)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            return gender;
        }

        public static string RaceSelection()
        {
            Console.WriteLine("(1) Human - Normal\n" +
                    "(2) Goblin - Normal\n" +
                    "(3) Elf - Normal\n" +
                    "(4) Beast - Normal\n" +
                    "(5) Demon - Ancient\n" +
                    "(6) Angel - Ancient");

            int selection = ValidateSelection(6);

            switch (selection)
            {
                case 1: return "Human";
                case 2: return "Goblin";
                case 3: return "Elf";
                case 4: return "Beast";
                case 5: return "Demon";
                case 6: return "Angel";
            }

            return "";
        }

        public static bool IsRoyalBlood()
        {
            bool confirmation = false;

            Console.WriteLine("(1) Yes\n" +
                    "(2) No");

            int selection = ValidateSelection(2);

            if (selection == 1)
            {
                confirmation = true;
            }
            else
            {
                confirmation = false;
            }

            return confirmation;
        }

        public static string DemonClassSelection()
        {
            string charClass = "";

            Console.WriteLine("(1) Mage\n" +
                            "(2) Fighter\n" +
                            "(3) Assassin\n" +
                            "(4) Marksman\n" +
                            "(5) Healer\n" +
                            "(6) Necromancer\n" +
                            "(7) Witch");

            int selection = ValidateSelection(7);   


            switch (selection)
            {
                case 1: return "Mage";
                case 2: return "Fighter";
                case 3: return "Assassin";
                case 4: return "Marksman";
                case 5: return "Healer";
                case 6: return "Necromancer";
                case 7: return "Witch";
            }

            return charClass;
        }

        public static string AngelClassSelection()
        {
            string charClass = "";

            Console.WriteLine("(1) Mage\n" +
                            "(2) Fighter\n" +
                            "(3) Assassin\n" +
                            "(4) Marksman\n" +
                            "(5) Healer\n" +
                            "(6) Paladin\n" +
                            "(7) Oracle");

            int selection = ValidateSelection(7);


            switch (selection)
            {
                case 1: return "Mage";
                case 2: return "Fighter";
                case 3: return "Assassin";
                case 4: return "Marksman";
                case 5: return "Healer";
                case 6: return "Paladin";
                case 7: return "Oracle";
            }

            return charClass;
        }
        public static string NormalClassSelection()
        {
            string charClass = "";
            Console.WriteLine("(1) Mage\n" +
                            "(2) Fighter\n" +
                            "(3) Assassin\n" +
                            "(4) Marksman\n" +
                            "(5) Healer");

            int selection = ValidateSelection(5);

            switch (selection)
            {
                case 1: return "Mage";
                case 2: return "Fighter";
                case 3: return "Assassin";
                case 4: return "Marksman";
                case 5: return "Healer";
            }

            return charClass;
        }
    }

    // Custom Exceptions
    public class CharacterCreationSystemException : Exception
    {
        public CharacterCreationSystemException()
        {

        }

        public CharacterCreationSystemException(string message) : base(message)
        {
            Console.WriteLine ($"CharacterCreationSystemException: {message}");
        }
    }

    public class CharacterDatabaseException : Exception
    {
        public CharacterDatabaseException()
        {

        }
        
        public CharacterDatabaseException (string message) : base(message)
        {
            Console.WriteLine($"CharacterDatabaseException: {message}");
        }
    }
}