﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectLight.Core.ProjectAggregate;
using ProjectLight.Core.ProjectAggregate.Specifications;
using ProjectLight.SharedKernel.Interfaces;
using ProjectLight.Web.ApiModels;

namespace ProjectLight.Web.Pages.ProjectDetails
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Project> _repository;

        [BindProperty(SupportsGet = true)]
        public int ProjectId { get; set; }

        public string Message { get; set; } = "";

        public ProjectDTO? Project { get; set; }

        public IndexModel(IRepository<Project> repository)
        {
            _repository = repository;
        }

        public async Task OnGetAsync()
        {
            var projectSpec = new ProjectByIdWithItemsSpec(ProjectId);
            var project = await _repository.FirstOrDefaultAsync(projectSpec);
            if (project == null)
            {
                Message = "No project found.";

                return;
            }

            Project = new ProjectDTO
            (
                id: project.Id,
                name: project.Name,
                items: project.Items
                .Select(item => ToDoItemDTO.FromToDoItem(item))
                .ToList()
            );
        }
    }
}