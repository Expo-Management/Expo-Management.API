using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Mentions;
using Expo_Management.API.Entities.Projects;
using Expo_Management.API.Interfaces;
using Expo_Management.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IFilesUploaderRepository _filesUploader;
        private readonly IUsersRepository _usersRepository;
        private CrudUtils _crudUtils;


        public ProjectsRepository(ApplicationDbContext context, IFilesUploaderRepository filesUploader, IUsersRepository usersRepository)
        {
            _context = context;
            _filesUploader = filesUploader;
            _usersRepository = usersRepository;
            _crudUtils = new CrudUtils(_usersRepository, _context);
        }


        public async Task<ProjectModel> CreateProject(NewProject model)
        {
            try
            {
                List<string> groupOfUserEmails = new List<string>();

                groupOfUserEmails.Add(model.Lider);
                groupOfUserEmails.Add(model.Member2);
                groupOfUserEmails.Add(model.Member3);

                var membersOfTheGroup = await this._crudUtils.getUsersAvailableAsync(groupOfUserEmails);
                var project = ProjectExists(model.Name);

                if (membersOfTheGroup != null && !project)
                {
                    var upload = await _filesUploader.AddProjectsFile(model.Files);
                    var Fair = await GetFair(model.Fair);

                    //create new project
                    ProjectModel newProject = new ProjectModel()
                    {
                        Name = model.Name,
                        Fair = Fair,
                        Description = model.Description,
                        Files = upload
                    };

                    var projectCreated = await _context.Projects.AddAsync(newProject);
                    await _context.SaveChangesAsync();

                    //add newProject to database

                    var ProjectCreated = await _crudUtils.addUsersToProject(groupOfUserEmails, newProject);

                    return newProject;

                }
                return null;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }

        }

        public async Task<List<ProjectModel>> GetAllProjectsAsync()
        {
            try
            {
                return await _context.Projects.Include(x => x.Files).ToListAsync();

            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }

        public async Task<Fair> GetFair(int fairId)
        {
            try
            {
                var result = await (from f in _context.Fair
                                    where f.Id == fairId
                                    select f).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }



        public bool ProjectExists(string name)
        {
            try
            {
                var result = (from x in _context.Projects
                              where x.Name == name
                              select x.Name).FirstOrDefault();

                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return false;
            }

        }

        public async Task<List<Mention>> GetMentionsAsync()
        {
            try
            {
                var mentions = await (from m in _context.Mention
                                      select m).ToListAsync();

                if (mentions != null && mentions.Count > 0)
                {
                    return mentions;
                }
                return null;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }

        public async Task<List<ProjectModel>> GetAllCurrentProjectsAsync()
        {
            try
            {
                var projects = await (from p in _context.Projects
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
                _context.Dispose();
                return null;
            }
        }

        public async Task<List<ProjectModel>> GetOldProjectsAsync()
        {
            try
            {
                var projects = await (from p in _context.Projects.
                                      Include(x => x.Fair)
                                      where p.Fair.StartDate.Year < DateTime.Now.Year
                                      select p).ToListAsync();

                if (projects != null && projects.Count > 0)
                {
                    return projects;
                }
                return null;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }

        public async Task<Claim> CreateProjectClaim(NewClaim model)
        {
            var project = await (from p in _context.Projects
                                 where p.Id == model.ProjectId
                                 select p).FirstOrDefaultAsync();

            if (project == null)
            {
                return null;
            }

            var claim = new Claim()
            {
                ClaimDescription = model.ClaimDescription,
                Project = project
            };

            await _context.Claim.AddAsync(claim);
            await _context.SaveChangesAsync();
            return claim;
        }

        public async Task<List<ProjectDetails>> GetProjectDetails(int projectId)
        {
            try
            {
                var projects = await (from p in _context.Projects
                                      where p.Id == projectId
                                      select new ProjectDetails()
                                      {
                                          ProjectId = p.Id,
                                          ProjectName = p.Name,
                                          ProjectDescription = p.Description,
                                          Members = null,
                                          Category = null,
                                          ProjectQualifications = null,
                                          FinalPunctuation = null,
                                      }).ToListAsync();

                if (projects != null)
                {
                    List<string> members = await GetProjectMembers(projectId);
                    List<ProjectQualifications> qualifications = await GetProjectQualifications(projectId);

                    projects[0].Members = members;
                    projects[0].ProjectQualifications = qualifications;
                    projects[0].FinalPunctuation = CalculateProjectFinalPunctuation(qualifications).Result.ToString();

                    return projects;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            async Task<List<string>> GetProjectMembers(int projectId)
            {
                return await (from u in _context.User
                              join p in _context.Projects on u.Project.Id equals p.Id
                              where p.Id == projectId
                              select u.Name + " " + u.Lastname).ToListAsync();
            }

            async Task<List<ProjectQualifications>> GetProjectQualifications(int projectId)
            {
                return await (from p in _context.Projects
                              join q in _context.Qualifications on p.Id equals q.Project.Id
                              join u in _context.User on q.Judge.Id equals u.Id
                              where p.Id == projectId
                              select new ProjectQualifications()
                              {
                                  Punctuation = q.Punctuation,
                                  JudgeName = u.Name + " " + u.Lastname
                              }).ToListAsync();
            }

            async Task<int> CalculateProjectFinalPunctuation(List<ProjectQualifications> qualifications)
            {
                if (qualifications != null && qualifications.Count() > 0)
                {
                    var FinalQualification = 0;
                    var counter = 0;

                    foreach (var judgeQualification in qualifications)
                    {
                        FinalQualification = FinalQualification + judgeQualification.Punctuation;
                        counter++;
                    }

                    return FinalQualification / counter;
                }
                return 0;

            }
        }

        public async Task<List<ProjectMembers>> GetMembers()
        {
            try
            {
                var members = await (from u in _context.User
                                     join p in _context.Projects on u.Project.Id equals p.Id
                                     select new ProjectMembers()
                                     {
                                         Name = u.Name,
                                         LastName = u.Lastname,
                                         Email = u.Email,
                                         PhoneNumber = u.PhoneNumber,
                                         ProjectId = p.Id,
                                         ProjectName = p.Name
                                     }).ToListAsync();

                if (members.Count > 0)
                {
                    return members;
                }
                return null;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }
    }
}