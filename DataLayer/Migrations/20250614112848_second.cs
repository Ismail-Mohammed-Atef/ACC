using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Folders_FolderId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentId",
                table: "DocumentVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCompany_Companies_CompanyId",
                table: "ProjectCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCompany_Projects_ProjectId",
                table: "ProjectCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_MemberId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDocuments_Documents_DocumentId",
                table: "ReviewDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDocuments_Reviews_ReviewId",
                table: "ReviewDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_InitiatorUserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_WorkflowTemplates_WorkflowTemplateId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsFolders_Folders_FolderId",
                table: "ReviewsFolders");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsFolders_Reviews_ReviewId",
                table: "ReviewsFolders");

            migrationBuilder.DropForeignKey(
                name: "FK_TransmittalDocuments_DocumentVersions_DocumentVersionId",
                table: "TransmittalDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TransmittalDocuments_Transmittals_TransmittalId",
                table: "TransmittalDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStepTemplates_WorkflowTemplates_WorkflowTemplateId",
                table: "WorkflowStepTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTemplates_Projects_ProjectId",
                table: "WorkflowTemplates");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Folders_FolderId",
                table: "Documents",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentId",
                table: "DocumentVersions",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCompany_Companies_CompanyId",
                table: "ProjectCompany",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCompany_Projects_ProjectId",
                table: "ProjectCompany",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_MemberId",
                table: "ProjectMembers",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDocuments_Documents_DocumentId",
                table: "ReviewDocuments",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDocuments_Reviews_ReviewId",
                table: "ReviewDocuments",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_InitiatorUserId",
                table: "Reviews",
                column: "InitiatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_WorkflowTemplates_WorkflowTemplateId",
                table: "Reviews",
                column: "WorkflowTemplateId",
                principalTable: "WorkflowTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsFolders_Folders_FolderId",
                table: "ReviewsFolders",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsFolders_Reviews_ReviewId",
                table: "ReviewsFolders",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TransmittalDocuments_DocumentVersions_DocumentVersionId",
                table: "TransmittalDocuments",
                column: "DocumentVersionId",
                principalTable: "DocumentVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TransmittalDocuments_Transmittals_TransmittalId",
                table: "TransmittalDocuments",
                column: "TransmittalId",
                principalTable: "Transmittals",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStepTemplates_WorkflowTemplates_WorkflowTemplateId",
                table: "WorkflowStepTemplates",
                column: "WorkflowTemplateId",
                principalTable: "WorkflowTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTemplates_Projects_ProjectId",
                table: "WorkflowTemplates",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Folders_FolderId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentId",
                table: "DocumentVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCompany_Companies_CompanyId",
                table: "ProjectCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCompany_Projects_ProjectId",
                table: "ProjectCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_MemberId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDocuments_Documents_DocumentId",
                table: "ReviewDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDocuments_Reviews_ReviewId",
                table: "ReviewDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_InitiatorUserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_WorkflowTemplates_WorkflowTemplateId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsFolders_Folders_FolderId",
                table: "ReviewsFolders");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsFolders_Reviews_ReviewId",
                table: "ReviewsFolders");

            migrationBuilder.DropForeignKey(
                name: "FK_TransmittalDocuments_DocumentVersions_DocumentVersionId",
                table: "TransmittalDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TransmittalDocuments_Transmittals_TransmittalId",
                table: "TransmittalDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStepTemplates_WorkflowTemplates_WorkflowTemplateId",
                table: "WorkflowStepTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTemplates_Projects_ProjectId",
                table: "WorkflowTemplates");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Folders_FolderId",
                table: "Documents",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentId",
                table: "DocumentVersions",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCompany_Companies_CompanyId",
                table: "ProjectCompany",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCompany_Projects_ProjectId",
                table: "ProjectCompany",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_MemberId",
                table: "ProjectMembers",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDocuments_Documents_DocumentId",
                table: "ReviewDocuments",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDocuments_Reviews_ReviewId",
                table: "ReviewDocuments",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_InitiatorUserId",
                table: "Reviews",
                column: "InitiatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_WorkflowTemplates_WorkflowTemplateId",
                table: "Reviews",
                column: "WorkflowTemplateId",
                principalTable: "WorkflowTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsFolders_Folders_FolderId",
                table: "ReviewsFolders",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsFolders_Reviews_ReviewId",
                table: "ReviewsFolders",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransmittalDocuments_DocumentVersions_DocumentVersionId",
                table: "TransmittalDocuments",
                column: "DocumentVersionId",
                principalTable: "DocumentVersions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransmittalDocuments_Transmittals_TransmittalId",
                table: "TransmittalDocuments",
                column: "TransmittalId",
                principalTable: "Transmittals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStepTemplates_WorkflowTemplates_WorkflowTemplateId",
                table: "WorkflowStepTemplates",
                column: "WorkflowTemplateId",
                principalTable: "WorkflowTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTemplates_Projects_ProjectId",
                table: "WorkflowTemplates",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
