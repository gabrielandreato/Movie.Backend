using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedMovieActorData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Title", "Genre" },
                values: new object[,]
                {
                    { "56ed483e-be99-4097-a404-86e25f6a6b72", "The Last Horizon", "Action" },
                    { "224a5add-fba3-408b-9102-b57ebe9d2cab", "Jungle Trail", "Adventure" },
                    { "d9c52d4f-deb9-4204-9ff5-82a7522c1170", "Whispers of Tomorrow", "Drama" },
                    { "c73e1d54-ba42-43fc-9453-2632b972e526", "Laugh Track", "Comedy" },
                    { "ee608800-c11d-4ce4-8306-8054c4626e55", "Midnight Heist", "Crime" },
                    { "475b8773-2cbd-4c2f-9de2-90fca26ef426", "Ocean Depths", "Documentary" },
                    { "fff2b09a-538c-48bb-ac82-b4b54574bdaf", "Silent Echoes", "Drama" },
                    { "4f8ed44c-2cd0-4641-a7f7-f54a4f8d4fe2", "Dragon's Realm", "Fantasy" },
                    { "9c907472-38bd-4ab3-9a44-99cc4da56296", "The Haunted Hollow", "Horror" },
                    { "735ce8b9-ca93-444f-9a3b-e1d5b30fe656", "Starlight Serenade", "Musical" },
                    { "1e00a95e-4301-4b26-8a0b-8a7cc28965a5", "The Vanishing Clue", "Mystery" },
                    { "bf1dfe66-8ed4-4cb9-9ec7-f5ab72dc6184", "Autumn Hearts", "Romance" },
                    { "764dd8c0-ea05-4f74-931b-fa3d13ab210f", "Beyond the Stars", "SciFi" },
                    { "a2782da8-6bd4-4456-a2d4-1b2b510e3d07", "Edge of Fear", "Thriller" },
                    { "918e9ea1-4510-4b6b-bb6f-14f73fa46165", "Fields of Valor", "War" },
                    { "c461656c-1920-43e1-ab26-415525e315bf", "Dust and Iron", "Western" },
                    { "3972516a-9d4f-471e-ac17-a39b01a4f8b8", "Rebel Skyline", "Action" },
                    { "df4ebb10-4160-4eea-a4f0-4fba8b77c50c", "Lost Kingdom", "Adventure" },
                    { "9ecdfddd-a8ec-4948-a73c-1e8f50eb5f95", "Toon Town Tales", "Animation" },
                    { "677f120a-48bf-4038-856d-7483b3b92c42", "Painted Skies", "Animation" },
                });

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "7444e35a-e88a-42dd-8224-158795d2a49b", "Ava Bennett" },
                    { "d256312c-9926-46de-b9eb-520a00e5b84a", "Liam Carter" },
                    { "00b115ca-c165-4932-a165-e356f3fbc763", "Sophia Diaz" },
                    { "4d3cefb0-068c-424d-b094-a070ecadf756", "Noah Ellis" },
                    { "7c9eea32-c6cf-4aa0-9532-15290ac5c4af", "Mia Foster" },
                    { "a17ad594-aa5c-4111-9545-9085d22d6277", "Ethan Grant" },
                    { "dba949f8-c96f-4eb1-b72a-fa21eda5a66b", "Isabella Hayes" },
                    { "938c7c32-1ea4-4584-9fd7-e762802b9d96", "Lucas Ingram" },
                    { "baf3f39d-fb01-4aae-8700-ca10c134f196", "Charlotte Jones" },
                    { "44458616-faa2-48d9-bf07-273c322ffdc0", "Mason Kelly" },
                    { "fa50457a-57d1-46f1-95fb-10dc549bd5f5", "Amelia Lane" },
                    { "6849b41c-8ad3-4436-aae5-ee927c2f0d32", "Benjamin Moore" },
                    { "48a562e8-0789-498b-b29b-0a719858449a", "Harper Nolan" },
                    { "0a22b365-e13f-4096-9fce-ff4dde7503cd", "James Owens" },
                    { "e6e65d9b-7859-4507-b3ba-e3481fe01deb", "Evelyn Parker" },
                    { "dcc08f6d-3d87-4eef-a8d5-364d1920bc4d", "Alexander Quinn" },
                    { "44fd18cd-be80-454a-8864-a3031ea072e3", "Abigail Reed" },
                    { "57f66e28-19e5-4e6c-9661-d4f4935239c5", "Daniel Stone" },
                    { "7d7a505f-765b-4160-b3ad-50652af53d12", "Emily Turner" },
                    { "f27e20e4-463f-4ea6-b351-882df5f1d06d", "Henry Vaughn" },
                });

            migrationBuilder.InsertData(
                table: "MovieActors",
                columns: new[] { "ActorsId", "MoviesId" },
                values: new object[,]
                {
                    { "4d3cefb0-068c-424d-b094-a070ecadf756", "56ed483e-be99-4097-a404-86e25f6a6b72" },
                    { "7444e35a-e88a-42dd-8224-158795d2a49b", "56ed483e-be99-4097-a404-86e25f6a6b72" },
                    { "baf3f39d-fb01-4aae-8700-ca10c134f196", "56ed483e-be99-4097-a404-86e25f6a6b72" },
                    { "938c7c32-1ea4-4584-9fd7-e762802b9d96", "56ed483e-be99-4097-a404-86e25f6a6b72" },
                    { "7c9eea32-c6cf-4aa0-9532-15290ac5c4af", "224a5add-fba3-408b-9102-b57ebe9d2cab" },
                    { "4d3cefb0-068c-424d-b094-a070ecadf756", "224a5add-fba3-408b-9102-b57ebe9d2cab" },
                    { "57f66e28-19e5-4e6c-9661-d4f4935239c5", "d9c52d4f-deb9-4204-9ff5-82a7522c1170" },
                    { "00b115ca-c165-4932-a165-e356f3fbc763", "d9c52d4f-deb9-4204-9ff5-82a7522c1170" },
                    { "0a22b365-e13f-4096-9fce-ff4dde7503cd", "d9c52d4f-deb9-4204-9ff5-82a7522c1170" },
                    { "d256312c-9926-46de-b9eb-520a00e5b84a", "d9c52d4f-deb9-4204-9ff5-82a7522c1170" },
                    { "00b115ca-c165-4932-a165-e356f3fbc763", "c73e1d54-ba42-43fc-9453-2632b972e526" },
                    { "dba949f8-c96f-4eb1-b72a-fa21eda5a66b", "c73e1d54-ba42-43fc-9453-2632b972e526" },
                    { "44fd18cd-be80-454a-8864-a3031ea072e3", "ee608800-c11d-4ce4-8306-8054c4626e55" },
                    { "7444e35a-e88a-42dd-8224-158795d2a49b", "ee608800-c11d-4ce4-8306-8054c4626e55" },
                    { "dba949f8-c96f-4eb1-b72a-fa21eda5a66b", "475b8773-2cbd-4c2f-9de2-90fca26ef426" },
                    { "57f66e28-19e5-4e6c-9661-d4f4935239c5", "475b8773-2cbd-4c2f-9de2-90fca26ef426" },
                    { "0a22b365-e13f-4096-9fce-ff4dde7503cd", "475b8773-2cbd-4c2f-9de2-90fca26ef426" },
                    { "938c7c32-1ea4-4584-9fd7-e762802b9d96", "475b8773-2cbd-4c2f-9de2-90fca26ef426" },
                    { "7d7a505f-765b-4160-b3ad-50652af53d12", "fff2b09a-538c-48bb-ac82-b4b54574bdaf" },
                    { "baf3f39d-fb01-4aae-8700-ca10c134f196", "fff2b09a-538c-48bb-ac82-b4b54574bdaf" },
                    { "7444e35a-e88a-42dd-8224-158795d2a49b", "fff2b09a-538c-48bb-ac82-b4b54574bdaf" },
                    { "0a22b365-e13f-4096-9fce-ff4dde7503cd", "4f8ed44c-2cd0-4641-a7f7-f54a4f8d4fe2" },
                    { "fa50457a-57d1-46f1-95fb-10dc549bd5f5", "4f8ed44c-2cd0-4641-a7f7-f54a4f8d4fe2" },
                    { "7c9eea32-c6cf-4aa0-9532-15290ac5c4af", "9c907472-38bd-4ab3-9a44-99cc4da56296" },
                    { "dba949f8-c96f-4eb1-b72a-fa21eda5a66b", "9c907472-38bd-4ab3-9a44-99cc4da56296" },
                    { "fa50457a-57d1-46f1-95fb-10dc549bd5f5", "9c907472-38bd-4ab3-9a44-99cc4da56296" },
                    { "00b115ca-c165-4932-a165-e356f3fbc763", "735ce8b9-ca93-444f-9a3b-e1d5b30fe656" },
                    { "48a562e8-0789-498b-b29b-0a719858449a", "735ce8b9-ca93-444f-9a3b-e1d5b30fe656" },
                    { "6849b41c-8ad3-4436-aae5-ee927c2f0d32", "1e00a95e-4301-4b26-8a0b-8a7cc28965a5" },
                    { "f27e20e4-463f-4ea6-b351-882df5f1d06d", "1e00a95e-4301-4b26-8a0b-8a7cc28965a5" },
                    { "baf3f39d-fb01-4aae-8700-ca10c134f196", "bf1dfe66-8ed4-4cb9-9ec7-f5ab72dc6184" },
                    { "d256312c-9926-46de-b9eb-520a00e5b84a", "bf1dfe66-8ed4-4cb9-9ec7-f5ab72dc6184" },
                    { "e6e65d9b-7859-4507-b3ba-e3481fe01deb", "bf1dfe66-8ed4-4cb9-9ec7-f5ab72dc6184" },
                    { "4d3cefb0-068c-424d-b094-a070ecadf756", "bf1dfe66-8ed4-4cb9-9ec7-f5ab72dc6184" },
                    { "00b115ca-c165-4932-a165-e356f3fbc763", "764dd8c0-ea05-4f74-931b-fa3d13ab210f" },
                    { "57f66e28-19e5-4e6c-9661-d4f4935239c5", "764dd8c0-ea05-4f74-931b-fa3d13ab210f" },
                    { "44458616-faa2-48d9-bf07-273c322ffdc0", "764dd8c0-ea05-4f74-931b-fa3d13ab210f" },
                    { "f27e20e4-463f-4ea6-b351-882df5f1d06d", "a2782da8-6bd4-4456-a2d4-1b2b510e3d07" },
                    { "6849b41c-8ad3-4436-aae5-ee927c2f0d32", "a2782da8-6bd4-4456-a2d4-1b2b510e3d07" },
                    { "dba949f8-c96f-4eb1-b72a-fa21eda5a66b", "a2782da8-6bd4-4456-a2d4-1b2b510e3d07" },
                    { "00b115ca-c165-4932-a165-e356f3fbc763", "a2782da8-6bd4-4456-a2d4-1b2b510e3d07" },
                    { "938c7c32-1ea4-4584-9fd7-e762802b9d96", "918e9ea1-4510-4b6b-bb6f-14f73fa46165" },
                    { "44458616-faa2-48d9-bf07-273c322ffdc0", "918e9ea1-4510-4b6b-bb6f-14f73fa46165" },
                    { "938c7c32-1ea4-4584-9fd7-e762802b9d96", "c461656c-1920-43e1-ab26-415525e315bf" },
                    { "4d3cefb0-068c-424d-b094-a070ecadf756", "c461656c-1920-43e1-ab26-415525e315bf" },
                    { "baf3f39d-fb01-4aae-8700-ca10c134f196", "3972516a-9d4f-471e-ac17-a39b01a4f8b8" },
                    { "e6e65d9b-7859-4507-b3ba-e3481fe01deb", "3972516a-9d4f-471e-ac17-a39b01a4f8b8" },
                    { "6849b41c-8ad3-4436-aae5-ee927c2f0d32", "3972516a-9d4f-471e-ac17-a39b01a4f8b8" },
                    { "6849b41c-8ad3-4436-aae5-ee927c2f0d32", "df4ebb10-4160-4eea-a4f0-4fba8b77c50c" },
                    { "f27e20e4-463f-4ea6-b351-882df5f1d06d", "df4ebb10-4160-4eea-a4f0-4fba8b77c50c" },
                    { "baf3f39d-fb01-4aae-8700-ca10c134f196", "677f120a-48bf-4038-856d-7483b3b92c42" },
                    { "00b115ca-c165-4932-a165-e356f3fbc763", "677f120a-48bf-4038-856d-7483b3b92c42" },
                    { "a17ad594-aa5c-4111-9545-9085d22d6277", "677f120a-48bf-4038-856d-7483b3b92c42" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"MovieActors\";");
            migrationBuilder.Sql("DELETE FROM \"Movies\";");
            migrationBuilder.Sql("DELETE FROM \"Actors\";");
        }
    }
}
