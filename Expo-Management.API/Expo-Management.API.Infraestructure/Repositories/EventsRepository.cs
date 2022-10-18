using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Castle.Core;
using Expo_Management.API.Domain.Models.Reponses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Abp.Json;
using System.Linq.Dynamic.Core;
using Expo_Management.API.Domain.InputModels;
using System.Reflection;
using Expo_Management.API.Domain.Models.ViewModels;

namespace Expo_Management.API.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio de eventos
    /// </summary>
    public class EventsRepository : IEventsRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<EventsRepository> _logger;

        /// <summary>
        /// Constructor del repositorio de eventos
        /// </summary>
        /// <param name="context"></param>
        public EventsRepository(ApplicationDbContext context, ILogger<EventsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Metodo para crear eventos
        /// </summary>
        /// <param name="Event"></param>
        /// <returns></returns>
        public async Task<Response> CreateEventAsync(EventInputModel Event)
        {
            try
            {
                var fair = await (from f in _context.Fair
                                  where f.Id == Event.FairId
                                  select f).FirstOrDefaultAsync();

                var kindEvent = await (from ke in _context.KindOfEvent
                                       where ke.Id == Event.KindEvent
                                       select ke).FirstOrDefaultAsync();

                if (fair != null && kindEvent != null)
                {
                    var newEvent = new Event()
                    {
                        Title = Event.Title,
                        Location = Event.Location,
                        Start = Event.Start,
                        End = Event.End,
                        Details = Event.Details,
                        AllDay = Event.AllDay,
                        KindEvents = kindEvent!,
                        Fair = fair
                    };

                    if (_context.Event.Add(newEvent) != null)
                    {
                        _context.SaveChanges();
                        return new Response()
                        {
                            Status = 200,
                            Data = newEvent,
                            Message = "Evento creado exitosamente!"
                        };
                    }
                    else
                    {
                        _logger.LogWarning("Error al crear un evento.");
                        return new Response()
                        {
                            Status = 500,
                            Message = "No se pudo crear el evento, intentelo mas tarde."
                        };
                    }
                }
                return new Response()
                {
                    Status = 400,
                    Message = "Revise los datos enviados"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para eliminar eventos
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEventAsync(int EventId)
        {
            var result = await (from e in _context.Event
                                where e.Id == EventId
                                select e).FirstOrDefaultAsync();
            try
            {
                if (result != null)
                {
                    _context.Event.Remove(result);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning("Error al eliminar un evento.");
                return false;
            }
        }

        /// <summary>
        /// Metodo para obtener un evento
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public async Task<Event?> GetEventAsync(int EventId)
        {
            var results = await (from e in _context.Event
                                 where e.Id == EventId
                                 select e).FirstOrDefaultAsync();

            if (results != null)
            {
                return (Event)results;
            }
            return null;
        }

        /// <summary>
        /// Metodo para obtener eventos
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetEventsAsync()
        {
            try
            {
                var results = await (from e in _context.Event
                                     select e)
                                     .Include(e => e.KindEvents)
                                     .Include(f => f.Fair)
                                     .ToListAsync();

                if (results.Any())
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = results
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "No hay eventos registrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para actualizar eventos
        /// </summary>
        /// <param name="Event"></param>
        /// <returns></returns>
        public async Task<Event?> UpdateEventAsync(EventUpdateInputModel Event)
        {
            var result = await (from e in _context.Event
                                where e.Id == Event.Id
                                select e).FirstOrDefaultAsync();

            var kindEvent = await (from ke in _context.KindOfEvent
                                   where ke.Id == Event.Id
                                   select ke).FirstOrDefaultAsync();

            if (result != null)
            {
                result.Title = Event.Title;
                result.Location = Event.Location;
                result.Start = Event.Start;
                result.End = Event.End;
                result.Details = Event.Details;
                result.AllDay = Event.AllDay;
                result.KindEvents = kindEvent!;
                _context.SaveChanges();

                return result;
            }
            _logger.LogWarning("Error al actualizar un evento.");
            return null;
        }

        /// <summary>
        /// Metodo para obtener los tipos de eventos
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public async Task<Response> GetKindEventsAsync()
        {
            try
            {
                var results = await (from ke in _context.KindOfEvent
                                     select ke)
                                     .ToListAsync();

                if (results.Any())
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = results,
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "No hay tipos de eventos registrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud contacte, administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener el valor hexadecimal de un color en especifico
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string? GetColorName(string name)
        {
            FieldInfo[] fields = typeof(Colors).GetFields();
            foreach (var field in fields)
            {
                if (field.Name == name)
                {
                    return field.GetValue(typeof(Colors)).ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Metodo para obtener el nombre y valor hexadecimal de un color en especifico
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Response GetColorName()
        {
            try
            {
               List<ColorsViewModel> colors = new();
                var fields = typeof(Colors).GetFields().ToList();

                foreach (var field in fields)
                {

                    var c = new ColorsViewModel()
                    {
                        Name = field.Name,
                        Value = field.GetValue(typeof(Colors)).ToString()
                    };
                    colors.Add(c);
                }
                return new Response()
                {
                    Status = 200,
                    Data = colors,
                    Message = "Colores obtenidos exitosamente!"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para crear tipos de categorias de los eventos de la feria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response?> CreateKindEventAsync(newKindEventInputModel model)
        {
            try
            {
                if (model != null)
                {
                    var primaryColour = GetColorName(model.Primary);
                    var secondaryColour = GetColorName(model.Secondary);

                    var newKindEvent = new KindEvents()
                    {
                        Name = model.Name,
                        Primary = primaryColour,
                        Secondary = secondaryColour
                    };

                    if (await _context.KindOfEvent.AddAsync(newKindEvent) != null)
                    {
                        await _context.SaveChangesAsync();
                        return new Response()
                        {
                            Status = 200,
                            Data = newKindEvent,
                            Message = "Tipo de Evento ha sido creado exitosamente!"
                        };
                    }
                }
                return new Response()
                {
                    Status = 400,
                    Message = "Revise los datos enviados"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para actualizar los tipos de eventos de la feria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response?> UpdateKindEventAsync(UpdateKindEventsInputModel model)
        {
            try
            {
                var result = await (from ke in _context.KindOfEvent
                                    where ke.Id == model.Id
                                    select ke).FirstOrDefaultAsync();


                var primaryColour = GetColorName(model.Primary);
                var secondaryColour = GetColorName(model.Secondary);

                if (result != null)
                {
                    result.Name = model.Name;
                    result.Primary = primaryColour;
                    result.Secondary = secondaryColour;
                    _context.SaveChanges();

                    return new Response()
                    {
                        Status = 200,
                        Data = result,
                        Message = "Tipo de Evento ha sido actualizado exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 400,
                    Message = "Revise los datos enviados"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para eliminar tipos de eventos de la feria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Response> DeleteKindEventAsync(int kindEventId)
        {
            var result = await (from e in _context.KindOfEvent
                                where e.Id == kindEventId
                                select e).FirstOrDefaultAsync();
            try
            {
                if (result != null)
                {
                    _context.KindOfEvent.Remove(result);
                    _context.SaveChanges();
                    return new Response()
                    {
                        Status = 200,
                        Data = true,
                        Message = "Tipo de Evento ha sido actualizado exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 400,
                    Data = false,
                    Message = "Revise los datos enviados"
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Data = false,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }


        /// <summary>
        /// Metodo para obtener las noticias de la feria
        /// </summary>
        /// <param name="FairId"></param>
        /// <returns></returns>
        public async Task<List<New>?> GetNewsByFairIdAsync(int FairId)
        {
            var results = await (from n in _context.New
                                 where n.Fair.Id == FairId
                                 select n).Include(n => n.Publisher).ToListAsync();

            if (results != null && results.Count() > 0)
            {
                return results;
            }
            return null;
        }

        /// <summary>
        /// Metodo para obtener los protocolos de seguridad
        /// </summary>
        /// <param name="FairId"></param>
        /// <returns></returns>
        public async Task<List<SecurityProtocols>?> GetGetSecurityProtocols(int FairId)
        {
            var results = await (from sp in _context.SecurityProtocols
                                 where sp.Fair.Id == FairId
                                 select sp).ToListAsync();

            if (results != null && results.Count() > 0)
            {
                return results;
            }
            return null;
        }
    }
}
