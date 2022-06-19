using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Mentions;
using Expo_Management.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {

        private readonly ApplicationDbContext context;
        private readonly IFilesUploaderRepository _filesUploader;

        public ProjectsRepository(ApplicationDbContext context, IFilesUploaderRepository filesUploader) {
            this.context = context;
            _filesUploader = filesUploader;
        }


        public async Task<ProjectModel> CreateProject(NewProject model)
        {
            try
            {
                var project = ProjectExists(model.Lider);


                if (!project)
                {

                    var upload = _filesUploader.AddProjectsFile(model.Files);
                     

                    //create new project
                    ProjectModel newProject = new ProjectModel()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Lider = model.Lider,
                        Member2 = model.Member2,
                        Member3 = model.Member3,
                        Files = await upload
                    };

                    //add newProject to database
                    await context.Projects.AddAsync(newProject);
                    await context.SaveChangesAsync();

                    return newProject;
                }

                return null;
            }
            catch (Exception ex)
            {
                context.Dispose();
                throw ex;
            }

        }

        public async Task<List<ProjectModel>> GetAllProjectsAsync()
        {
            try
            {
                return await context.Projects.Include(x => x.Files).ToListAsync();

            }
            catch (Exception ex)
            {
                context.Dispose();
                return null;
            }
        }

        public bool ProjectExists(string lider)
        {
            try
            {
                var result = (from X in context.Projects
                              where X.Lider == lider
                              select X).FirstOrDefault();

                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                context.Dispose();
                return false;
            }

        }


        public async Task<List<Mention>> GetMentionsAsync() 
        {
            try
            {
                var mentions = await (from m in context.Mention
                                      select m).ToListAsync();

                if (mentions != null && mentions.Count > 0)
                {
                    return mentions;
                }
                return null;
            }
            catch (Exception ex)
            {
                context.Dispose();
                return null;
            }
        }

        public async Task<List<ProjectModel>> GetAllCurrentProjectsAsync()
        {
            try
            {
                var projects = await (from p in context.Projects
                                      where p.Fair.StartDate.Year == DateTime.Now.Year
                                      select p).ToListAsync();

                if (projects != null && projects.Count > 0)
                {
                    return projects;
                }
                return null;
            }
            catch (Exception ex)
            {
                context.Dispose();
                return null;
            }
        }
    }
 }