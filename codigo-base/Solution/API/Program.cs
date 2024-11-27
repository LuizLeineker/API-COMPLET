using API.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
/*TASK */

/*CREATE*/
app.MapPost("/api/tarefas/cadastro", ([FromBody] Tarefa tarefa, [FromServices] AppDataContext ctx) =>{
    ctx.Tarefas.Add(tarefa);
    ctx.SaveChanges();
    return Results.Created("", tarefa);

}
);

/*SEARCH*/
app.MapGet("/api/tarefas/buscar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>{
    Tarefa? tarefa = ctx.Tarefas.Find(id);
    if(tarefa is null){
        return Results.NotFound("Nenhuma tarefa encontrada");
    }

    return Results.Ok(tarefa);
}
);

/*LIST*/
app.MapGet("/api/tarefas/listar", ([FromServices] AppDataContext ctx) =>{
    if(ctx.Tarefas.Count() == 0){
        return Results.NotFound("Nenhuma tarefa encontrada");
    }

    return Results.Ok(ctx.Tarefas.ToList());

}
);

/*DELETE*/
app.MapDelete("/api/tarefas/remove/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>{
    Tarefa? tarefa = ctx.Tarefas.Find(id);
    if(tarefa is null){
        return Results.NotFound("Nenhuma tarefa encontrada");
    }
    ctx.Tarefas.Remove(tarefa);
    ctx.SaveChanges();
    return Results.Ok(tarefa);
}
);

/*UPDATE*/
app.MapPut("/api/tarefas/atualizar/{id}", ([FromRoute] int id, [FromBody] Tarefa tarefa, [FromServices] AppDataContext ctx) =>{
    Tarefa? tarefaAtual = ctx.Tarefas.Find(id);
    if(tarefaAtual is null){
        return Results.NotFound("Nenhuma tarefa encontrada");
    }
    tarefaAtual.Nome = tarefa.Nome;
    ctx.Tarefas.Update(tarefaAtual);
    ctx.SaveChanges();
    return Results.Ok(tarefaAtual);
}
);





app.Run();