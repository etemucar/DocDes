using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocDes.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "language",
                columns: table => new
                {
                    language_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    language_cd = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language", x => x.language_id);
                });

            migrationBuilder.CreateTable(
                name: "localizable_fields",
                columns: table => new
                {
                    localizable_fields_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    entity_field = table.Column<string>(type: "text", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localizable_fields", x => x.localizable_fields_id);
                });

            migrationBuilder.CreateTable(
                name: "localization",
                columns: table => new
                {
                    localization_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    entity_id = table.Column<int>(type: "integer", nullable: false),
                    entity_field = table.Column<string>(type: "text", nullable: false),
                    language_cd = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localization", x => x.localization_id);
                });

            migrationBuilder.CreateTable(
                name: "party",
                columns: table => new
                {
                    party_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_party", x => x.party_id);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    city_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    coordinates = table.Column<string>(type: "text", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.city_id);
                    table.ForeignKey(
                        name: "FK_city_country_country_id",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contact_medium",
                columns: table => new
                {
                    contact_medium_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    party_id = table.Column<int>(type: "integer", nullable: false),
                    medium_type = table.Column<int>(type: "integer", nullable: false),
                    is_preferred = table.Column<bool>(type: "boolean", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    address_id = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact_medium", x => x.contact_medium_id);
                    table.ForeignKey(
                        name: "FK_contact_medium_party_party_id",
                        column: x => x.party_id,
                        principalTable: "party",
                        principalColumn: "party_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "individual",
                columns: table => new
                {
                    individual_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    party_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_individual", x => x.individual_id);
                    table.ForeignKey(
                        name: "FK_individual_party_party_id",
                        column: x => x.party_id,
                        principalTable: "party",
                        principalColumn: "party_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organization",
                columns: table => new
                {
                    organization_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    party_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    tax_office = table.Column<string>(type: "text", nullable: true),
                    tax_number = table.Column<long>(type: "bigint", nullable: false),
                    identity_number = table.Column<long>(type: "bigint", nullable: false),
                    trade_name = table.Column<string>(type: "text", nullable: true),
                    trade_register_number = table.Column<long>(type: "bigint", nullable: false),
                    mersis_no = table.Column<long>(type: "bigint", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization", x => x.organization_id);
                    table.ForeignKey(
                        name: "FK_organization_party_party_id",
                        column: x => x.party_id,
                        principalTable: "party",
                        principalColumn: "party_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "related_party",
                columns: table => new
                {
                    related_party_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    party_id = table.Column<int>(type: "integer", nullable: false),
                    related_to_party_id = table.Column<int>(type: "integer", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    valid_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    valid_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_related_party", x => x.related_party_id);
                    table.ForeignKey(
                        name: "FK_related_party_party_party_id",
                        column: x => x.party_id,
                        principalTable: "party",
                        principalColumn: "party_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_related_party_party_related_to_party_id",
                        column: x => x.related_to_party_id,
                        principalTable: "party",
                        principalColumn: "party_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "district",
                columns: table => new
                {
                    district_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    coordinates = table.Column<string>(type: "text", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_district", x => x.district_id);
                    table.ForeignKey(
                        name: "FK_district_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "city_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organization_language_rel",
                columns: table => new
                {
                    organization_language_rel_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organization_id = table.Column<int>(type: "integer", nullable: false),
                    language_cd = table.Column<string>(type: "text", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_language_rel", x => x.organization_language_rel_id);
                    table.ForeignKey(
                        name: "FK_organization_language_rel_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "language_id");
                    table.ForeignKey(
                        name: "FK_organization_language_rel_organization_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organization",
                        principalColumn: "organization_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "party_role_type",
                columns: table => new
                {
                    party_role_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organization_id = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_party_role_type", x => x.party_role_type_id);
                    table.ForeignKey(
                        name: "FK_party_role_type_organization_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organization",
                        principalColumn: "organization_id");
                });

            migrationBuilder.CreateTable(
                name: "town",
                columns: table => new
                {
                    town_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    district_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    zip_code = table.Column<string>(type: "text", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_town", x => x.town_id);
                    table.ForeignKey(
                        name: "FK_town_district_district_id",
                        column: x => x.district_id,
                        principalTable: "district",
                        principalColumn: "district_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "party_role",
                columns: table => new
                {
                    party_role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    party_id = table.Column<int>(type: "integer", nullable: false),
                    party_role_type_id = table.Column<int>(type: "integer", nullable: false),
                    organization_id = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_party_role", x => x.party_role_id);
                    table.ForeignKey(
                        name: "FK_party_role_organization_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organization",
                        principalColumn: "organization_id");
                    table.ForeignKey(
                        name: "FK_party_role_party_party_id",
                        column: x => x.party_id,
                        principalTable: "party",
                        principalColumn: "party_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_party_role_party_role_type_party_role_type_id",
                        column: x => x.party_role_type_id,
                        principalTable: "party_role_type",
                        principalColumn: "party_role_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "neighborhood",
                columns: table => new
                {
                    neighborhood_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    town_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_neighborhood", x => x.neighborhood_id);
                    table.ForeignKey(
                        name: "FK_neighborhood_town_town_id",
                        column: x => x.town_id,
                        principalTable: "town",
                        principalColumn: "town_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    party_role_id = table.Column<int>(type: "integer", nullable: false),
                    customer_number = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.customer_id);
                    table.ForeignKey(
                        name: "FK_customer_party_role_party_role_id",
                        column: x => x.party_role_id,
                        principalTable: "party_role",
                        principalColumn: "party_role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "digital_identity",
                columns: table => new
                {
                    digital_identity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nickname = table.Column<string>(type: "text", nullable: true),
                    digital_identity_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    party_role_id = table.Column<int>(type: "integer", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_digital_identity", x => x.digital_identity_id);
                    table.ForeignKey(
                        name: "FK_digital_identity_party_role_party_role_id",
                        column: x => x.party_role_id,
                        principalTable: "party_role",
                        principalColumn: "party_role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    address_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    district_id = table.Column<int>(type: "integer", nullable: false),
                    town_id = table.Column<int>(type: "integer", nullable: true),
                    neighborhood_id = table.Column<int>(type: "integer", nullable: true),
                    street = table.Column<string>(type: "text", nullable: true),
                    postal_code = table.Column<string>(type: "text", nullable: true),
                    street_line1 = table.Column<string>(type: "text", nullable: true),
                    street_line2 = table.Column<string>(type: "text", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.address_id);
                    table.ForeignKey(
                        name: "FK_address_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "city_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_address_country_country_id",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_address_district_district_id",
                        column: x => x.district_id,
                        principalTable: "district",
                        principalColumn: "district_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_address_neighborhood_neighborhood_id",
                        column: x => x.neighborhood_id,
                        principalTable: "neighborhood",
                        principalColumn: "neighborhood_id");
                    table.ForeignKey(
                        name: "FK_address_town_town_id",
                        column: x => x.town_id,
                        principalTable: "town",
                        principalColumn: "town_id");
                });

            migrationBuilder.CreateTable(
                name: "party_role_account",
                columns: table => new
                {
                    party_role_account_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    party_role_id = table.Column<int>(type: "integer", nullable: false),
                    currency_code = table.Column<string>(type: "text", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_party_role_account", x => x.party_role_account_id);
                    table.ForeignKey(
                        name: "FK_party_role_account_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "customer_id");
                    table.ForeignKey(
                        name: "FK_party_role_account_party_role_party_role_id",
                        column: x => x.party_role_id,
                        principalTable: "party_role",
                        principalColumn: "party_role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_user",
                columns: table => new
                {
                    application_user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    external_user_id = table.Column<string>(type: "text", nullable: true),
                    party_role_id = table.Column<int>(type: "integer", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    digital_identity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_user", x => x.application_user_id);
                    table.ForeignKey(
                        name: "FK_application_user_digital_identity_digital_identity_id",
                        column: x => x.digital_identity_id,
                        principalTable: "digital_identity",
                        principalColumn: "digital_identity_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_user_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "language_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_user_organization_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organization",
                        principalColumn: "organization_id");
                    table.ForeignKey(
                        name: "FK_application_user_party_role_party_role_id",
                        column: x => x.party_role_id,
                        principalTable: "party_role",
                        principalColumn: "party_role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credential",
                columns: table => new
                {
                    credential_id = table.Column<Guid>(type: "uuid", nullable: false),
                    credential_type = table.Column<int>(type: "integer", nullable: false),
                    trust_level = table.Column<int>(type: "integer", nullable: true),
                    digital_identity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credential", x => x.credential_id);
                    table.ForeignKey(
                        name: "FK_credential_digital_identity_digital_identity_id",
                        column: x => x.digital_identity_id,
                        principalTable: "digital_identity",
                        principalColumn: "digital_identity_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    refresh_token_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token_hash = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    created_by_ip = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    application_user_id = table.Column<int>(type: "integer", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.refresh_token_id);
                    table.ForeignKey(
                        name: "FK_refresh_token_application_user_application_user_id",
                        column: x => x.application_user_id,
                        principalTable: "application_user",
                        principalColumn: "application_user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credential_characteristic",
                columns: table => new
                {
                    credential_characteristic_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    credential_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credential_characteristic", x => x.credential_characteristic_id);
                    table.ForeignKey(
                        name: "FK_credential_characteristic_credential_credential_id",
                        column: x => x.credential_id,
                        principalTable: "credential",
                        principalColumn: "credential_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_city_id",
                table: "address",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_address_country_id",
                table: "address",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_address_district_id",
                table: "address",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "IX_address_neighborhood_id",
                table: "address",
                column: "neighborhood_id");

            migrationBuilder.CreateIndex(
                name: "IX_address_town_id",
                table: "address",
                column: "town_id");

            migrationBuilder.CreateIndex(
                name: "IX_application_user_digital_identity_id",
                table: "application_user",
                column: "digital_identity_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_application_user_language_id",
                table: "application_user",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_application_user_organization_id",
                table: "application_user",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_application_user_party_role_id",
                table: "application_user",
                column: "party_role_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_city_country_id",
                table: "city",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_contact_medium_party_id",
                table: "contact_medium",
                column: "party_id");

            migrationBuilder.CreateIndex(
                name: "IX_credential_digital_identity_id",
                table: "credential",
                column: "digital_identity_id");

            migrationBuilder.CreateIndex(
                name: "IX_credential_characteristic_credential_id",
                table: "credential_characteristic",
                column: "credential_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_party_role_id",
                table: "customer",
                column: "party_role_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_digital_identity_party_role_id",
                table: "digital_identity",
                column: "party_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_district_city_id",
                table: "district",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_individual_party_id",
                table: "individual",
                column: "party_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_neighborhood_town_id",
                table: "neighborhood",
                column: "town_id");

            migrationBuilder.CreateIndex(
                name: "IX_organization_party_id",
                table: "organization",
                column: "party_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_organization_language_rel_language_id",
                table: "organization_language_rel",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_organization_language_rel_organization_id",
                table: "organization_language_rel",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_party_role_organization_id",
                table: "party_role",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_party_role_party_id",
                table: "party_role",
                column: "party_id");

            migrationBuilder.CreateIndex(
                name: "IX_party_role_party_role_type_id",
                table: "party_role",
                column: "party_role_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_party_role_account_customer_id",
                table: "party_role_account",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_party_role_account_party_role_id",
                table: "party_role_account",
                column: "party_role_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_party_role_type_organization_id",
                table: "party_role_type",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_application_user_id",
                table: "refresh_token",
                column: "application_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_related_party_party_id",
                table: "related_party",
                column: "party_id");

            migrationBuilder.CreateIndex(
                name: "IX_related_party_related_to_party_id",
                table: "related_party",
                column: "related_to_party_id");

            migrationBuilder.CreateIndex(
                name: "IX_town_district_id",
                table: "town",
                column: "district_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "contact_medium");

            migrationBuilder.DropTable(
                name: "credential_characteristic");

            migrationBuilder.DropTable(
                name: "individual");

            migrationBuilder.DropTable(
                name: "localizable_fields");

            migrationBuilder.DropTable(
                name: "localization");

            migrationBuilder.DropTable(
                name: "organization_language_rel");

            migrationBuilder.DropTable(
                name: "party_role_account");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "related_party");

            migrationBuilder.DropTable(
                name: "neighborhood");

            migrationBuilder.DropTable(
                name: "credential");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "application_user");

            migrationBuilder.DropTable(
                name: "town");

            migrationBuilder.DropTable(
                name: "digital_identity");

            migrationBuilder.DropTable(
                name: "language");

            migrationBuilder.DropTable(
                name: "district");

            migrationBuilder.DropTable(
                name: "party_role");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "party_role_type");

            migrationBuilder.DropTable(
                name: "country");

            migrationBuilder.DropTable(
                name: "organization");

            migrationBuilder.DropTable(
                name: "party");
        }
    }
}
