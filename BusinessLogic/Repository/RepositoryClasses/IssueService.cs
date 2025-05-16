using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models; 
using BusinessLogic.Repository.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;

        public IssueService(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public void CreateIssue(Issue issue)
        {
            if (string.IsNullOrEmpty(issue.Title))
                throw new Exception("Title cannot be empty.");
            if (string.IsNullOrEmpty(issue.Description))
                throw new Exception("Description cannot be empty.");

            _issueRepository.Add(issue);
        }

        public Issue GetIssueById(int id)
        {
            var issue = _issueRepository.GetById(id);
            if (issue == null)
                throw new Exception("Issue not found.");
            return issue;
        }

        public void UpdateIssue(Issue issue)
        {
            var existingIssue = _issueRepository.GetById(issue.Id);
            if (existingIssue == null)
                throw new Exception("Issue not found.");

            
            existingIssue.Title = issue.Title;
            existingIssue.Description = issue.Description;
            existingIssue.Category = issue.Category;
            existingIssue.Type = issue.Type;
            existingIssue.Priority = issue.Priority;
            existingIssue.Status = issue.Status;
            existingIssue.ProjectId = issue.ProjectId;

            _issueRepository.Update(existingIssue);
        }

        public void DeleteIssue(int id)
        {
            var issue = _issueRepository.GetById(id);
            if (issue == null)
                throw new Exception("Issue not found.");
            _issueRepository.Delete(id);
        }

        public List<Issue> GetAllIssues()
        {
            return _issueRepository.GetAll();
        }
        public List<Issue> GetIssuesByProjectId(int projectId)
        {
            return _issueRepository.GetIssuesByProjectId(projectId);
        }

    }
}
